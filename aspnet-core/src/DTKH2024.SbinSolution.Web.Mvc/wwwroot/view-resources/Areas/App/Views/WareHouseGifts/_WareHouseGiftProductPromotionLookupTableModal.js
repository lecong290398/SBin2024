﻿(function ($) {
  app.modals.ProductPromotionLookupTableModal = function () {
    var _modalManager;

    var _wareHouseGiftsService = abp.services.app.wareHouseGifts;
    var _$productPromotionTable = $('#ProductPromotionTable');

    this.init = function (modalManager) {
      _modalManager = modalManager;
    };

    var dataTable = _$productPromotionTable.DataTable({
      paging: true,
      serverSide: true,
      processing: true,
      listAction: {
        ajaxFunction: _wareHouseGiftsService.getAllProductPromotionForLookupTable,
        inputFilter: function () {
          return {
            filter: $('#ProductPromotionTableFilter').val(),
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

    $('#ProductPromotionTable tbody').on('click', '[id*=selectbtn]', function () {
      var data = dataTable.row($(this).parents('tr')).data();
      _modalManager.setResult(data);
      _modalManager.close();
    });

    function getProductPromotion() {
      dataTable.ajax.reload();
    }

    $('#GetProductPromotionButton').click(function (e) {
      e.preventDefault();
      getProductPromotion();
    });

    $('#SelectButton').click(function (e) {
      e.preventDefault();
    });

    $('#ProductPromotionTableFilter').keypress(function (e) {
      if (e.which === 13 && e.target.tagName.toLocaleLowerCase() != 'textarea') {
        getProductPromotion();
      }
    });
  };
})(jQuery);
