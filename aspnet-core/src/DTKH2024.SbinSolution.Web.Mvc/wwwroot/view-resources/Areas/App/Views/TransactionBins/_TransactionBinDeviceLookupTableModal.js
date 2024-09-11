(function ($) {
  app.modals.DeviceLookupTableModal = function () {
    var _modalManager;

    var _transactionBinsService = abp.services.app.transactionBins;
    var _$deviceTable = $('#DeviceTable');

    this.init = function (modalManager) {
      _modalManager = modalManager;
    };

    var dataTable = _$deviceTable.DataTable({
      paging: true,
      serverSide: true,
      processing: true,
      listAction: {
        ajaxFunction: _transactionBinsService.getAllDeviceForLookupTable,
        inputFilter: function () {
          return {
            filter: $('#DeviceTableFilter').val(),
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

    $('#DeviceTable tbody').on('click', '[id*=selectbtn]', function () {
      var data = dataTable.row($(this).parents('tr')).data();
      _modalManager.setResult(data);
      _modalManager.close();
    });

    function getDevice() {
      dataTable.ajax.reload();
    }

    $('#GetDeviceButton').click(function (e) {
      e.preventDefault();
      getDevice();
    });

    $('#SelectButton').click(function (e) {
      e.preventDefault();
    });

    $('#DeviceTableFilter').keypress(function (e) {
      if (e.which === 13 && e.target.tagName.toLocaleLowerCase() != 'textarea') {
        getDevice();
      }
    });
  };
})(jQuery);
