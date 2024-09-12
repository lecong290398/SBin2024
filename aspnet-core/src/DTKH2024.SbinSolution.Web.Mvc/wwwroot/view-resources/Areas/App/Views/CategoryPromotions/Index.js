﻿(function () {
  $(function () {
    var _$categoryPromotionsTable = $('#CategoryPromotionsTable');
    var _categoryPromotionsService = abp.services.app.categoryPromotions;

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
        getCategoryPromotions();
      })
      .on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        $selectedDate.startDate = null;
        getCategoryPromotions();
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
        getCategoryPromotions();
      })
      .on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        $selectedDate.endDate = null;
        getCategoryPromotions();
      });

    var _permissions = {
      create: abp.auth.hasPermission('Pages.Administration.CategoryPromotions.Create'),
      edit: abp.auth.hasPermission('Pages.Administration.CategoryPromotions.Edit'),
      delete: abp.auth.hasPermission('Pages.Administration.CategoryPromotions.Delete'),
    };

    var _createOrEditModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/CategoryPromotions/CreateOrEditModal',
      scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/CategoryPromotions/_CreateOrEditModal.js',
      modalClass: 'CreateOrEditCategoryPromotionModal',
    });

    var _viewCategoryPromotionModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/CategoryPromotions/ViewcategoryPromotionModal',
      modalClass: 'ViewCategoryPromotionModal',
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

    var dataTable = _$categoryPromotionsTable.DataTable({
      paging: true,
      serverSide: true,
      processing: true,
      listAction: {
        ajaxFunction: _categoryPromotionsService.getAll,
        inputFilter: function () {
          return {
            filter: $('#CategoryPromotionsTableFilter').val(),
            nameFilter: $('#NameFilterId').val(),
            colorFilter: $('#ColorFilterId').val(),
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
                  _viewCategoryPromotionModal.open({ id: data.record.categoryPromotion.id });
                },
              },
              {
                text: app.localize('Edit'),
                visible: function () {
                  return _permissions.edit;
                },
                action: function (data) {
                  _createOrEditModal.open({ id: data.record.categoryPromotion.id });
                },
              },
              {
                text: app.localize('Delete'),
                visible: function () {
                  return _permissions.delete;
                },
                action: function (data) {
                  deleteCategoryPromotion(data.record.categoryPromotion);
                },
              },
            ],
          },
        },
        {
          targets: 2,
          data: 'categoryPromotion.name',
          name: 'name',
        },
        {
          targets: 3,
          data: 'categoryPromotion.description',
          name: 'description',
        },
        {
          targets: 4,
          data: 'categoryPromotion.color',
          name: 'color',
        },
      ],
    });

    function getCategoryPromotions() {
      dataTable.ajax.reload();
    }

    function deleteCategoryPromotion(categoryPromotion) {
      abp.message.confirm('', app.localize('AreYouSure'), function (isConfirmed) {
        if (isConfirmed) {
          _categoryPromotionsService
            .delete({
              id: categoryPromotion.id,
            })
            .done(function () {
              getCategoryPromotions(true);
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

    $('#CreateNewCategoryPromotionButton').click(function () {
      _createOrEditModal.open();
    });

    $('#ExportToExcelButton').click(function () {
      _categoryPromotionsService
        .getCategoryPromotionsToExcel({
          filter: $('#CategoryPromotionsTableFilter').val(),
          nameFilter: $('#NameFilterId').val(),
          colorFilter: $('#ColorFilterId').val(),
        })
        .done(function (result) {
          app.downloadTempFile(result);
        });
    });

    abp.event.on('app.createOrEditCategoryPromotionModalSaved', function () {
      getCategoryPromotions();
    });

    $('#GetCategoryPromotionsButton').click(function (e) {
      e.preventDefault();
      getCategoryPromotions();
    });

    $(document).keypress(function (e) {
      if (e.which === 13) {
        getCategoryPromotions();
      }
    });

    $('.reload-on-change').change(function (e) {
      getCategoryPromotions();
    });

    $('.reload-on-keyup').keyup(function (e) {
      getCategoryPromotions();
    });

    $('#btn-reset-filters').click(function (e) {
      $('.reload-on-change,.reload-on-keyup,#MyEntsTableFilter').val('');
      getCategoryPromotions();
    });
  });
})();