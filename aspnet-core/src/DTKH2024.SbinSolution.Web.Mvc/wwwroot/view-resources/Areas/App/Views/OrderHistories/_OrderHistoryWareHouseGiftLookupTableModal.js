﻿(function ($) {
  app.modals.WareHouseGiftLookupTableModal = function () {
    var _modalManager;

    var _orderHistoriesService = abp.services.app.orderHistories;
    var _$wareHouseGiftTable = $('#WareHouseGiftTable');

    this.init = function (modalManager) {
      _modalManager = modalManager;
    };

    var dataTable = _$wareHouseGiftTable.DataTable({
      paging: true,
      serverSide: true,
      processing: true,
      listAction: {
        ajaxFunction: _orderHistoriesService.getAllWareHouseGiftForLookupTable,
        inputFilter: function () {
          return {
            filter: $('#WareHouseGiftTableFilter').val(),
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

    $('#WareHouseGiftTable tbody').on('click', '[id*=selectbtn]', function () {
      var data = dataTable.row($(this).parents('tr')).data();
      _modalManager.setResult(data);
      _modalManager.close();
    });

    function getWareHouseGift() {
      dataTable.ajax.reload();
    }

    $('#GetWareHouseGiftButton').click(function (e) {
      e.preventDefault();
      getWareHouseGift();
    });

    $('#SelectButton').click(function (e) {
      e.preventDefault();
    });

    $('#WareHouseGiftTableFilter').keypress(function (e) {
      if (e.which === 13 && e.target.tagName.toLocaleLowerCase() != 'textarea') {
        getWareHouseGift();
      }
    });
  };
})(jQuery);
