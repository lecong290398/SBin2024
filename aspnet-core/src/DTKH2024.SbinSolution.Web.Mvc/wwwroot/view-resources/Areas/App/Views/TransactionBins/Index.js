﻿(function () {
  $(function () {
    var _$transactionBinsTable = $('#TransactionBinsTable');
    var _transactionBinsService = abp.services.app.transactionBins;

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
        getTransactionBins();
      })
      .on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        $selectedDate.startDate = null;
        getTransactionBins();
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
        getTransactionBins();
      })
      .on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        $selectedDate.endDate = null;
        getTransactionBins();
      });

    var _permissions = {
      create: abp.auth.hasPermission('Pages.Administration.TransactionBins.Create'),
      edit: abp.auth.hasPermission('Pages.Administration.TransactionBins.Edit'),
      delete: abp.auth.hasPermission('Pages.Administration.TransactionBins.Delete'),
    };

    var _createOrEditModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/TransactionBins/CreateOrEditModal',
      scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/TransactionBins/_CreateOrEditModal.js',
      modalClass: 'CreateOrEditTransactionBinModal',
    });

    var _viewTransactionBinModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/TransactionBins/ViewtransactionBinModal',
      modalClass: 'ViewTransactionBinModal',
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

    var dataTable = _$transactionBinsTable.DataTable({
      paging: true,
      serverSide: true,
      processing: true,
      listAction: {
        ajaxFunction: _transactionBinsService.getAll,
        inputFilter: function () {
          return {
            filter: $('#TransactionBinsTableFilter').val(),
            transactionCodeFilter: $('#TransactionCodeFilterId').val(),
            deviceNameFilter: $('#DeviceNameFilterId').val(),
            userNameFilter: $('#UserNameFilterId').val(),
            transactionStatusNameFilter: $('#TransactionStatusNameFilterId').val(),
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
                  _viewTransactionBinModal.open({ id: data.record.transactionBin.id });
                },
              },
              {
                text: app.localize('Edit'),
                visible: function () {
                  return _permissions.edit;
                },
                action: function (data) {
                  _createOrEditModal.open({ id: data.record.transactionBin.id });
                },
              },
              {
                text: app.localize('Delete'),
                visible: function () {
                  return _permissions.delete;
                },
                action: function (data) {
                  deleteTransactionBin(data.record.transactionBin);
                },
              },
            ],
          },
        },
        {
          targets: 2,
          data: 'transactionBin.plastisQuantity',
          name: 'plastisQuantity',
        },
        {
          targets: 3,
          data: 'transactionBin.plastisPoint',
          name: 'plastisPoint',
        },
        {
          targets: 4,
          data: 'transactionBin.metalQuantity',
          name: 'metalQuantity',
        },
        {
          targets: 5,
          data: 'transactionBin.metalPoint',
          name: 'metalPoint',
        },
        {
          targets: 6,
          data: 'transactionBin.ortherQuantity',
          name: 'ortherQuantity',
        },
        {
          targets: 7,
          data: 'transactionBin.errorPoint',
          name: 'errorPoint',
        },
        {
          targets: 8,
          data: 'transactionBin.transactionCode',
          name: 'transactionCode',
        },
        {
          targets: 9,
          data: 'deviceName',
          name: 'deviceFk.name',
        },
        {
          targets: 10,
          data: 'userName',
          name: 'userFk.name',
        },
        {
          targets: 11,
          data: 'transactionStatusName',
          name: 'transactionStatusFk.name',
          },
          {
              targets: 12,
              data: 'transactionBin.creationTime',
              name: 'creationTime',
          },
      ],
    });

    function getTransactionBins() {
      dataTable.ajax.reload();
    }

    function deleteTransactionBin(transactionBin) {
      abp.message.confirm('', app.localize('AreYouSure'), function (isConfirmed) {
        if (isConfirmed) {
          _transactionBinsService
            .delete({
              id: transactionBin.id,
            })
            .done(function () {
              getTransactionBins(true);
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

    $('#CreateNewTransactionBinButton').click(function () {
      _createOrEditModal.open();
    });

    $('#ExportToExcelButton').click(function () {
      _transactionBinsService
        .getTransactionBinsToExcel({
          filter: $('#TransactionBinsTableFilter').val(),
          transactionCodeFilter: $('#TransactionCodeFilterId').val(),
          deviceNameFilter: $('#DeviceNameFilterId').val(),
          userNameFilter: $('#UserNameFilterId').val(),
          transactionStatusNameFilter: $('#TransactionStatusNameFilterId').val(),
        })
        .done(function (result) {
          app.downloadTempFile(result);
        });
    });

    abp.event.on('app.createOrEditTransactionBinModalSaved', function () {
      getTransactionBins();
    });

    $('#GetTransactionBinsButton').click(function (e) {
      e.preventDefault();
      getTransactionBins();
    });

    $(document).keypress(function (e) {
      if (e.which === 13) {
        getTransactionBins();
      }
    });

    $('.reload-on-change').change(function (e) {
      getTransactionBins();
    });

    $('.reload-on-keyup').keyup(function (e) {
      getTransactionBins();
    });

    $('#btn-reset-filters').click(function (e) {
      $('.reload-on-change,.reload-on-keyup,#MyEntsTableFilter').val('');
      getTransactionBins();
    });
  });
})();
