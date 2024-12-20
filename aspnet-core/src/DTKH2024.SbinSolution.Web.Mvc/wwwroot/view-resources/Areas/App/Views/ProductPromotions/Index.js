﻿(function () {
  $(function () {
    var _$productPromotionsTable = $('#ProductPromotionsTable');
    var _productPromotionsService = abp.services.app.productPromotions;

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
        getProductPromotions();
      })
      .on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        $selectedDate.startDate = null;
        getProductPromotions();
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
        getProductPromotions();
      })
      .on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        $selectedDate.endDate = null;
        getProductPromotions();
      });

    var _permissions = {
      create: abp.auth.hasPermission('Pages.Administration.ProductPromotions.Create'),
      edit: abp.auth.hasPermission('Pages.Administration.ProductPromotions.Edit'),
      delete: abp.auth.hasPermission('Pages.Administration.ProductPromotions.Delete'),
    };

    var _createOrEditModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/ProductPromotions/CreateOrEditModal',
      scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/ProductPromotions/_CreateOrEditModal.js',
      modalClass: 'CreateOrEditProductPromotionModal',
    });

    var _viewProductPromotionModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/ProductPromotions/ViewproductPromotionModal',
      modalClass: 'ViewProductPromotionModal',
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

    var dataTable = _$productPromotionsTable.DataTable({
      paging: true,
      serverSide: true,
      processing: true,
      listAction: {
        ajaxFunction: _productPromotionsService.getAll,
        inputFilter: function () {
          return {
            filter: $('#ProductPromotionsTableFilter').val(),
            minPointFilter: $('#MinPointFilterId').val(),
            maxPointFilter: $('#MaxPointFilterId').val(),
            minStartDateFilter: getDateFilter($('#MinStartDateFilterId')),
            maxStartDateFilter: getMaxDateFilter($('#MaxStartDateFilterId')),
            minEndDateFilter: getDateFilter($('#MinEndDateFilterId')),
            maxEndDateFilter: getMaxDateFilter($('#MaxEndDateFilterId')),
            promotionCodeFilter: $('#PromotionCodeFilterId').val(),
            productProductNameFilter: $('#ProductProductNameFilterId').val(),
            categoryPromotionNameFilter: $('#CategoryPromotionNameFilterId').val(),
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
                  _viewProductPromotionModal.open({ id: data.record.productPromotion.id });
                },
              },
              {
                text: app.localize('Edit'),
                visible: function () {
                  return _permissions.edit;
                },
                action: function (data) {
                  _createOrEditModal.open({ id: data.record.productPromotion.id });
                },
              },
              {
                text: app.localize('Delete'),
                visible: function () {
                  return _permissions.delete;
                },
                action: function (data) {
                  deleteProductPromotion(data.record.productPromotion);
                },
              },
            ],
          },
        },
        {
          targets: 2,
          data: 'productPromotion.point',
          name: 'point',
        },
        {
          targets: 3,
          data: 'productPromotion.quantityCurrent',
          name: 'quantityCurrent',
        },
        {
          targets: 4,
          data: 'productPromotion.quantityInStock',
          name: 'quantityInStock',
        },
        {
          targets: 5,
          data: 'productPromotion.startDate',
          name: 'startDate',
          render: function (startDate) {
            if (startDate) {
              return moment(startDate).format('L');
            }
            return '';
          },
        },
        {
          targets: 6,
          data: 'productPromotion.endDate',
          name: 'endDate',
          render: function (endDate) {
            if (endDate) {
              return moment(endDate).format('L');
            }
            return '';
          },
        },
        {
          targets: 7,
          data: 'productPromotion.promotionCode',
          name: 'promotionCode',
        },
        {
          targets: 8,
          data: 'productPromotion.description',
          name: 'description',
        },
        {
          targets: 9,
          data: 'productProductName',
          name: 'productFk.productName',
        },
        {
          targets: 10,
          data: 'categoryPromotionName',
          name: 'categoryPromotionFk.name',
        },
      ],
    });

    function getProductPromotions() {
      dataTable.ajax.reload();
    }

    function deleteProductPromotion(productPromotion) {
      abp.message.confirm('', app.localize('AreYouSure'), function (isConfirmed) {
        if (isConfirmed) {
          _productPromotionsService
            .delete({
              id: productPromotion.id,
            })
            .done(function () {
              getProductPromotions(true);
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

    $('#CreateNewProductPromotionButton').click(function () {
      _createOrEditModal.open();
    });

    $('#ExportToExcelButton').click(function () {
      _productPromotionsService
        .getProductPromotionsToExcel({
          filter: $('#ProductPromotionsTableFilter').val(),
          minPointFilter: $('#MinPointFilterId').val(),
          maxPointFilter: $('#MaxPointFilterId').val(),
          minStartDateFilter: getDateFilter($('#MinStartDateFilterId')),
          maxStartDateFilter: getMaxDateFilter($('#MaxStartDateFilterId')),
          minEndDateFilter: getDateFilter($('#MinEndDateFilterId')),
          maxEndDateFilter: getMaxDateFilter($('#MaxEndDateFilterId')),
          promotionCodeFilter: $('#PromotionCodeFilterId').val(),
          productProductNameFilter: $('#ProductProductNameFilterId').val(),
          categoryPromotionNameFilter: $('#CategoryPromotionNameFilterId').val(),
        })
        .done(function (result) {
          app.downloadTempFile(result);
        });
    });

    abp.event.on('app.createOrEditProductPromotionModalSaved', function () {
      getProductPromotions();
    });

    $('#GetProductPromotionsButton').click(function (e) {
      e.preventDefault();
      getProductPromotions();
    });

    $(document).keypress(function (e) {
      if (e.which === 13) {
        getProductPromotions();
      }
    });

    $('.reload-on-change').change(function (e) {
      getProductPromotions();
    });

    $('.reload-on-keyup').keyup(function (e) {
      getProductPromotions();
    });

    $('#btn-reset-filters').click(function (e) {
      $('.reload-on-change,.reload-on-keyup,#MyEntsTableFilter').val('');
      getProductPromotions();
    });
  });
})();
