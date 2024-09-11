(function ($) {
  app.modals.TransactionBinLookupTableModal = function () {
    var _modalManager;

    var _orderHistoriesService = abp.services.app.orderHistories;
    var _$transactionBinTable = $('#TransactionBinTable');

    this.init = function (modalManager) {
      _modalManager = modalManager;
    };

    var dataTable = _$transactionBinTable.DataTable({
      paging: true,
      serverSide: true,
      processing: true,
      listAction: {
        ajaxFunction: _orderHistoriesService.getAllTransactionBinForLookupTable,
        inputFilter: function () {
          return {
            filter: $('#TransactionBinTableFilter').val(),
          };
        },
      },
      columnDefs: [
        {
          targets: 0,
          data: null,
          orderable: false,
          autoWidth: false,
          defaultContent:
            "<div class=\"text-center\"><input id='selectbtn' class='btn btn-success' type='button' width='25px' value='" +
            app.localize('Select') +
            "' /></div>",
        },
        {
          autoWidth: false,
          orderable: false,
          targets: 1,
          data: 'displayName',
        },
      ],
    });

    $('#TransactionBinTable tbody').on('click', '[id*=selectbtn]', function () {
      var data = dataTable.row($(this).parents('tr')).data();
      _modalManager.setResult(data);
      _modalManager.close();
    });

    function getTransactionBin() {
      dataTable.ajax.reload();
    }

    $('#GetTransactionBinButton').click(function (e) {
      e.preventDefault();
      getTransactionBin();
    });

    $('#SelectButton').click(function (e) {
      e.preventDefault();
    });

    $('#TransactionBinTableFilter').keypress(function (e) {
      if (e.which === 13 && e.target.tagName.toLocaleLowerCase() != 'textarea') {
        getTransactionBin();
      }
    });
  };
})(jQuery);
