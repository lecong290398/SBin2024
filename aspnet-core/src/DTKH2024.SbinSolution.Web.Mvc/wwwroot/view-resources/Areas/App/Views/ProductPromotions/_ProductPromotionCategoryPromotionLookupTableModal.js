(function ($) {
  app.modals.CategoryPromotionLookupTableModal = function () {
    var _modalManager;

    var _productPromotionsService = abp.services.app.productPromotions;
    var _$categoryPromotionTable = $('#CategoryPromotionTable');

    this.init = function (modalManager) {
      _modalManager = modalManager;
    };

    var dataTable = _$categoryPromotionTable.DataTable({
      paging: true,
      serverSide: true,
      processing: true,
      listAction: {
        ajaxFunction: _productPromotionsService.getAllCategoryPromotionForLookupTable,
        inputFilter: function () {
          return {
            filter: $('#CategoryPromotionTableFilter').val(),
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

    $('#CategoryPromotionTable tbody').on('click', '[id*=selectbtn]', function () {
      var data = dataTable.row($(this).parents('tr')).data();
      _modalManager.setResult(data);
      _modalManager.close();
    });

    function getCategoryPromotion() {
      dataTable.ajax.reload();
    }

    $('#GetCategoryPromotionButton').click(function (e) {
      e.preventDefault();
      getCategoryPromotion();
    });

    $('#SelectButton').click(function (e) {
      e.preventDefault();
    });

    $('#CategoryPromotionTableFilter').keypress(function (e) {
      if (e.which === 13 && e.target.tagName.toLocaleLowerCase() != 'textarea') {
        getCategoryPromotion();
      }
    });
  };
})(jQuery);
