(function () {
  $(function () {
    var _$rankLevelsTable = $('#RankLevelsTable');
    var _rankLevelsService = abp.services.app.rankLevels;

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
        getRankLevels();
      })
      .on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        $selectedDate.startDate = null;
        getRankLevels();
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
        getRankLevels();
      })
      .on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        $selectedDate.endDate = null;
        getRankLevels();
      });

    var _permissions = {
      create: abp.auth.hasPermission('Pages.Administration.RankLevels.Create'),
      edit: abp.auth.hasPermission('Pages.Administration.RankLevels.Edit'),
      delete: abp.auth.hasPermission('Pages.Administration.RankLevels.Delete'),
    };

    var _createOrEditModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/RankLevels/CreateOrEditModal',
      scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/RankLevels/_CreateOrEditModal.js',
      modalClass: 'CreateOrEditRankLevelModal',
    });

    var _viewRankLevelModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/RankLevels/ViewrankLevelModal',
      modalClass: 'ViewRankLevelModal',
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

    var dataTable = _$rankLevelsTable.DataTable({
      paging: true,
      serverSide: true,
      processing: true,
      listAction: {
        ajaxFunction: _rankLevelsService.getAll,
        inputFilter: function () {
          return {
            filter: $('#RankLevelsTableFilter').val(),
            nameFilter: $('#NameFilterId').val(),
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
                  _viewRankLevelModal.open({ id: data.record.rankLevel.id });
                },
              },
              {
                text: app.localize('Edit'),
                visible: function () {
                  return _permissions.edit;
                },
                action: function (data) {
                  _createOrEditModal.open({ id: data.record.rankLevel.id });
                },
              },
              {
                text: app.localize('Delete'),
                visible: function () {
                  return _permissions.delete;
                },
                action: function (data) {
                  deleteRankLevel(data.record.rankLevel);
                },
              },
            ],
          },
        },
        {
          targets: 2,
          data: 'rankLevel.name',
          name: 'name',
        },
        {
          targets: 3,
          data: 'rankLevel.description',
          name: 'description',
        },
        {
          targets: 4,
          data: 'rankLevel.minimumPositiveScore',
          name: 'minimumPositiveScore',
        },
        {
          targets: 5,
          data: 'rankLevel.color',
          name: 'color',
        },
        {
          targets: 6,
          data: 'rankLevel',
          render: function (rankLevel) {
            if (!rankLevel.logo) {
              return '';
            }
            return `<a href="/File/DownloadBinaryFile?id=${rankLevel.logo}" target="_blank">${rankLevel.logoFileName}</a>`;
          },
        },
      ],
    });

    function getRankLevels() {
      dataTable.ajax.reload();
    }

    function deleteRankLevel(rankLevel) {
      abp.message.confirm('', app.localize('AreYouSure'), function (isConfirmed) {
        if (isConfirmed) {
          _rankLevelsService
            .delete({
              id: rankLevel.id,
            })
            .done(function () {
              getRankLevels(true);
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

    $('#CreateNewRankLevelButton').click(function () {
      _createOrEditModal.open();
    });

    $('#ExportToExcelButton').click(function () {
      _rankLevelsService
        .getRankLevelsToExcel({
          filter: $('#RankLevelsTableFilter').val(),
          nameFilter: $('#NameFilterId').val(),
        })
        .done(function (result) {
          app.downloadTempFile(result);
        });
    });

    abp.event.on('app.createOrEditRankLevelModalSaved', function () {
      getRankLevels();
    });

    $('#GetRankLevelsButton').click(function (e) {
      e.preventDefault();
      getRankLevels();
    });

    $(document).keypress(function (e) {
      if (e.which === 13) {
        getRankLevels();
      }
    });

    $('.reload-on-change').change(function (e) {
      getRankLevels();
    });

    $('.reload-on-keyup').keyup(function (e) {
      getRankLevels();
    });

    $('#btn-reset-filters').click(function (e) {
      $('.reload-on-change,.reload-on-keyup,#MyEntsTableFilter').val('');
      getRankLevels();
    });
  });
})();
