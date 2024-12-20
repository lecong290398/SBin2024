﻿(function () {
  $(function () {
    var _$benefitsRankLevelsTable = $('#BenefitsRankLevelsTable');
    var _benefitsRankLevelsService = abp.services.app.benefitsRankLevels;

    var $selectedDate = {
      startDate: null,
      endDate: null,
    };

    $('.date-picker').on('apply.daterangepicker', function (ev, picker) {
      $(this).val(picker.startDate.format('MM/DD/YYYY'));
    });

    $('.startDate')
      .daterangepicker({
        autoUpdateInput: false,
        singleDatePicker: true,
        locale: abp.localization.currentLanguage.name,
        format: 'L',
      })
      .on('apply.daterangepicker', (ev, picker) => {
        $selectedDate.startDate = picker.startDate;
        getBenefitsRankLevels();
      })
      .on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        $selectedDate.startDate = null;
        getBenefitsRankLevels();
      });

    $('.endDate')
      .daterangepicker({
        autoUpdateInput: false,
        singleDatePicker: true,
        locale: abp.localization.currentLanguage.name,
        format: 'L',
      })
      .on('apply.daterangepicker', (ev, picker) => {
        $selectedDate.endDate = picker.startDate;
        getBenefitsRankLevels();
      })
      .on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        $selectedDate.endDate = null;
        getBenefitsRankLevels();
      });

    var _permissions = {
      create: abp.auth.hasPermission('Pages.Administration.BenefitsRankLevels.Create'),
      edit: abp.auth.hasPermission('Pages.Administration.BenefitsRankLevels.Edit'),
      delete: abp.auth.hasPermission('Pages.Administration.BenefitsRankLevels.Delete'),
    };

    var _createOrEditModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/BenefitsRankLevels/CreateOrEditModal',
      scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/BenefitsRankLevels/_CreateOrEditModal.js',
      modalClass: 'CreateOrEditBenefitsRankLevelModal',
    });

    var _viewBenefitsRankLevelModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/BenefitsRankLevels/ViewbenefitsRankLevelModal',
      modalClass: 'ViewBenefitsRankLevelModal',
    });

    var getDateFilter = function (element) {
      if ($selectedDate.startDate == null) {
        return null;
      }
      return $selectedDate.startDate.format('YYYY-MM-DDT00:00:00Z');
    };

    var getMaxDateFilter = function (element) {
      if ($selectedDate.endDate == null) {
        return null;
      }
      return $selectedDate.endDate.format('YYYY-MM-DDT23:59:59Z');
    };

    var dataTable = _$benefitsRankLevelsTable.DataTable({
      paging: true,
      serverSide: true,
      processing: true,
      listAction: {
        ajaxFunction: _benefitsRankLevelsService.getAll,
        inputFilter: function () {
          return {
            filter: $('#BenefitsRankLevelsTableFilter').val(),
            nameFilter: $('#NameFilterId').val(),
            rankLevelNameFilter: $('#RankLevelNameFilterId').val(),
          };
        },
      },
      columnDefs: [
        {
          className: 'control responsive',
          orderable: false,
          render: function () {
            return '';
          },
          targets: 0,
        },
        {
          width: 120,
          targets: 1,
          data: null,
          orderable: false,
          autoWidth: false,
          defaultContent: '',
          rowAction: {
            cssClass: 'btn btn-brand dropdown-toggle',
            text: '<i class="fa fa-cog"></i> ' + app.localize('Actions') + ' <span class="caret"></span>',
            items: [
              {
                text: app.localize('View'),
                action: function (data) {
                  _viewBenefitsRankLevelModal.open({ id: data.record.benefitsRankLevel.id });
                },
              },
              {
                text: app.localize('Edit'),
                visible: function () {
                  return _permissions.edit;
                },
                action: function (data) {
                  _createOrEditModal.open({ id: data.record.benefitsRankLevel.id });
                },
              },
              {
                text: app.localize('Delete'),
                visible: function () {
                  return _permissions.delete;
                },
                action: function (data) {
                  deleteBenefitsRankLevel(data.record.benefitsRankLevel);
                },
              },
            ],
          },
        },
        {
          targets: 2,
          data: 'benefitsRankLevel.name',
          name: 'name',
        },
        {
          targets: 3,
          data: 'benefitsRankLevel.description',
          name: 'description',
        },
        {
          targets: 4,
          data: 'rankLevelName',
          name: 'rankLevelFk.name',
        },
      ],
    });

    function getBenefitsRankLevels() {
      dataTable.ajax.reload();
    }

    function deleteBenefitsRankLevel(benefitsRankLevel) {
      abp.message.confirm('', app.localize('AreYouSure'), function (isConfirmed) {
        if (isConfirmed) {
          _benefitsRankLevelsService
            .delete({
              id: benefitsRankLevel.id,
            })
            .done(function () {
              getBenefitsRankLevels(true);
              abp.notify.success(app.localize('SuccessfullyDeleted'));
            });
        }
      });
    }

    $('#ShowAdvancedFiltersSpan').click(function () {
      $('#ShowAdvancedFiltersSpan').hide();
      $('#HideAdvancedFiltersSpan').show();
      $('#AdvacedAuditFiltersArea').slideDown();
    });

    $('#HideAdvancedFiltersSpan').click(function () {
      $('#HideAdvancedFiltersSpan').hide();
      $('#ShowAdvancedFiltersSpan').show();
      $('#AdvacedAuditFiltersArea').slideUp();
    });

    $('#CreateNewBenefitsRankLevelButton').click(function () {
      _createOrEditModal.open();
    });

    $('#ExportToExcelButton').click(function () {
      _benefitsRankLevelsService
        .getBenefitsRankLevelsToExcel({
          filter: $('#BenefitsRankLevelsTableFilter').val(),
          nameFilter: $('#NameFilterId').val(),
          rankLevelNameFilter: $('#RankLevelNameFilterId').val(),
        })
        .done(function (result) {
          app.downloadTempFile(result);
        });
    });

    abp.event.on('app.createOrEditBenefitsRankLevelModalSaved', function () {
      getBenefitsRankLevels();
    });

    $('#GetBenefitsRankLevelsButton').click(function (e) {
      e.preventDefault();
      getBenefitsRankLevels();
    });

    $(document).keypress(function (e) {
      if (e.which === 13) {
        getBenefitsRankLevels();
      }
    });

    $('.reload-on-change').change(function (e) {
      getBenefitsRankLevels();
    });

    $('.reload-on-keyup').keyup(function (e) {
      getBenefitsRankLevels();
    });

    $('#btn-reset-filters').click(function (e) {
      $('.reload-on-change,.reload-on-keyup,#MyEntsTableFilter').val('');
      getBenefitsRankLevels();
    });
  });
})();
