(function () {
  $(function () {
    var _$wareHouseGiftsTable = $('#WareHouseGiftsTable');
    var _wareHouseGiftsService = abp.services.app.wareHouseGifts;

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
        getWareHouseGifts();
      })
      .on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        $selectedDate.startDate = null;
        getWareHouseGifts();
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
        getWareHouseGifts();
      })
      .on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        $selectedDate.endDate = null;
        getWareHouseGifts();
      });

    var _permissions = {
      create: abp.auth.hasPermission('Pages.WareHouseGifts.Create'),
      edit: abp.auth.hasPermission('Pages.WareHouseGifts.Edit'),
      delete: abp.auth.hasPermission('Pages.WareHouseGifts.Delete'),
    };

    var _createOrEditModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/WareHouseGifts/CreateOrEditModal',
      scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/WareHouseGifts/_CreateOrEditModal.js',
      modalClass: 'CreateOrEditWareHouseGiftModal',
    });

    var _viewWareHouseGiftModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/WareHouseGifts/ViewwareHouseGiftModal',
      modalClass: 'ViewWareHouseGiftModal',
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

    var dataTable = _$wareHouseGiftsTable.DataTable({
      paging: true,
      serverSide: true,
      processing: true,
      listAction: {
        ajaxFunction: _wareHouseGiftsService.getAll,
        inputFilter: function () {
          return {
            filter: $('#WareHouseGiftsTableFilter').val(),
            codeFilter: $('#CodeFilterId').val(),
            isUsedFilter: $('#IsUsedFilterId').val(),
            userNameFilter: $('#UserNameFilterId').val(),
            productPromotionPromotionCodeFilter: $('#ProductPromotionPromotionCodeFilterId').val(),
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
                  _viewWareHouseGiftModal.open({ id: data.record.wareHouseGift.id });
                },
              },
              {
                text: app.localize('Edit'),
                visible: function () {
                  return _permissions.edit;
                },
                action: function (data) {
                  _createOrEditModal.open({ id: data.record.wareHouseGift.id });
                },
              },
              {
                text: app.localize('Delete'),
                visible: function () {
                  return _permissions.delete;
                },
                action: function (data) {
                  deleteWareHouseGift(data.record.wareHouseGift);
                },
              },
            ],
          },
        },
        {
          targets: 2,
          data: 'wareHouseGift.code',
          name: 'code',
        },
        {
          targets: 3,
          data: 'wareHouseGift.isUsed',
          name: 'isUsed',
          render: function (isUsed) {
            if (isUsed) {
              return '<div class="text-center"><i class="fa fa-check text-success" title="True"></i></div>';
            }
            return '<div class="text-center"><i class="fa fa-times-circle" title="False"></i></div>';
          },
        },
        {
          targets: 4,
          data: 'userName',
          name: 'userFk.name',
        },
        {
          targets: 5,
          data: 'productPromotionPromotionCode',
          name: 'productPromotionFk.promotionCode',
        },
      ],
    });

    function getWareHouseGifts() {
      dataTable.ajax.reload();
    }

    function deleteWareHouseGift(wareHouseGift) {
      abp.message.confirm('', app.localize('AreYouSure'), function (isConfirmed) {
        if (isConfirmed) {
          _wareHouseGiftsService
            .delete({
              id: wareHouseGift.id,
            })
            .done(function () {
              getWareHouseGifts(true);
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

    $('#CreateNewWareHouseGiftButton').click(function () {
      _createOrEditModal.open();
    });

    $('#ExportToExcelButton').click(function () {
      _wareHouseGiftsService
        .getWareHouseGiftsToExcel({
          filter: $('#WareHouseGiftsTableFilter').val(),
          codeFilter: $('#CodeFilterId').val(),
          isUsedFilter: $('#IsUsedFilterId').val(),
          userNameFilter: $('#UserNameFilterId').val(),
          productPromotionPromotionCodeFilter: $('#ProductPromotionPromotionCodeFilterId').val(),
        })
        .done(function (result) {
          app.downloadTempFile(result);
        });
    });

    abp.event.on('app.createOrEditWareHouseGiftModalSaved', function () {
      getWareHouseGifts();
    });

    $('#GetWareHouseGiftsButton').click(function (e) {
      e.preventDefault();
      getWareHouseGifts();
    });

    $(document).keypress(function (e) {
      if (e.which === 13) {
        getWareHouseGifts();
      }
    });

    $('.reload-on-change').change(function (e) {
      getWareHouseGifts();
    });

    $('.reload-on-keyup').keyup(function (e) {
      getWareHouseGifts();
    });

    $('#btn-reset-filters').click(function (e) {
      $('.reload-on-change,.reload-on-keyup,#MyEntsTableFilter').val('');
      getWareHouseGifts();
    });
  });
})();
