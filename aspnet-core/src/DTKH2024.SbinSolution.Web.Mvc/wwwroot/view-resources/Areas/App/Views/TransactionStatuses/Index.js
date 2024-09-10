(function () {
  $(function () {
    var _$transactionStatusesTable = $('#TransactionStatusesTable');
    var _transactionStatusesService = abp.services.app.transactionStatuses;

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
        getTransactionStatuses();
      })
      .on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        $selectedDate.startDate = null;
        getTransactionStatuses();
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
        getTransactionStatuses();
      })
      .on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        $selectedDate.endDate = null;
        getTransactionStatuses();
      });

    var _permissions = {
      create: abp.auth.hasPermission('Pages.Administration.TransactionStatuses.Create'),
      edit: abp.auth.hasPermission('Pages.Administration.TransactionStatuses.Edit'),
      delete: abp.auth.hasPermission('Pages.Administration.TransactionStatuses.Delete'),
    };

    var _createOrEditModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/TransactionStatuses/CreateOrEditModal',
      scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/TransactionStatuses/_CreateOrEditModal.js',
      modalClass: 'CreateOrEditTransactionStatusModal',
    });

    var _viewTransactionStatusModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/TransactionStatuses/ViewtransactionStatusModal',
      modalClass: 'ViewTransactionStatusModal',
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

    var dataTable = _$transactionStatusesTable.DataTable({
      paging: true,
      serverSide: true,
      processing: true,
      listAction: {
        ajaxFunction: _transactionStatusesService.getAll,
        inputFilter: function () {
          return {
            filter: $('#TransactionStatusesTableFilter').val(),
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
                  _viewTransactionStatusModal.open({ id: data.record.transactionStatus.id });
                },
              },
              {
                text: app.localize('Edit'),
                visible: function () {
                  return _permissions.edit;
                },
                action: function (data) {
                  _createOrEditModal.open({ id: data.record.transactionStatus.id });
                },
              },
              {
                text: app.localize('Delete'),
                visible: function () {
                  return _permissions.delete;
                },
                action: function (data) {
                  deleteTransactionStatus(data.record.transactionStatus);
                },
              },
            ],
          },
        },
        {
          targets: 2,
          data: 'transactionStatus.name',
          name: 'name',
        },
        {
          targets: 3,
          data: 'transactionStatus.description',
          name: 'description',
        },
        {
          targets: 4,
          data: 'transactionStatus.color',
          name: 'color',
        },
      ],
    });

    function getTransactionStatuses() {
      dataTable.ajax.reload();
    }

    function deleteTransactionStatus(transactionStatus) {
      abp.message.confirm('', app.localize('AreYouSure'), function (isConfirmed) {
        if (isConfirmed) {
          _transactionStatusesService
            .delete({
              id: transactionStatus.id,
            })
            .done(function () {
              getTransactionStatuses(true);
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

    $('#CreateNewTransactionStatusButton').click(function () {
      _createOrEditModal.open();
    });

    $('#ExportToExcelButton').click(function () {
      _transactionStatusesService
        .getTransactionStatusesToExcel({
          filter: $('#TransactionStatusesTableFilter').val(),
          nameFilter: $('#NameFilterId').val(),
        })
        .done(function (result) {
          app.downloadTempFile(result);
        });
    });

    abp.event.on('app.createOrEditTransactionStatusModalSaved', function () {
      getTransactionStatuses();
    });

    $('#GetTransactionStatusesButton').click(function (e) {
      e.preventDefault();
      getTransactionStatuses();
    });

    $(document).keypress(function (e) {
      if (e.which === 13) {
        getTransactionStatuses();
      }
    });

    $('.reload-on-change').change(function (e) {
      getTransactionStatuses();
    });

    $('.reload-on-keyup').keyup(function (e) {
      getTransactionStatuses();
    });

    $('#btn-reset-filters').click(function (e) {
      $('.reload-on-change,.reload-on-keyup,#MyEntsTableFilter').val('');
      getTransactionStatuses();
    });
  });
})();
