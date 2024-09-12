(function ($) {
  app.modals.CreateOrEditWareHouseGiftModal = function () {
    var _wareHouseGiftsService = abp.services.app.wareHouseGifts;

    var _modalManager;
    var _$wareHouseGiftInformationForm = null;

    var _WareHouseGiftuserLookupTableModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/WareHouseGifts/UserLookupTableModal',
      scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/WareHouseGifts/_WareHouseGiftUserLookupTableModal.js',
      modalClass: 'UserLookupTableModal',
    });
    var _WareHouseGiftproductPromotionLookupTableModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/WareHouseGifts/ProductPromotionLookupTableModal',
      scriptUrl:
        abp.appPath + 'view-resources/Areas/App/Views/WareHouseGifts/_WareHouseGiftProductPromotionLookupTableModal.js',
      modalClass: 'ProductPromotionLookupTableModal',
    });

    this.init = function (modalManager) {
      _modalManager = modalManager;

      var modal = _modalManager.getModal();
      modal.find('.date-picker').daterangepicker({
        singleDatePicker: true,
        locale: abp.localization.currentLanguage.name,
        format: 'L',
      });

      // Init select2
      var typeaheadUserId = $('#userId-typeahead-selector');

      typeaheadUserId.select2({
        placeholder: 'Select',
        theme: 'bootstrap5',
        selectionCssClass: 'form-select',
        dropdownParent: _modalManager.getModal(),
        minimumInputLength: 2,
        ajax: {
          url: abp.appPath + 'api/services/app/Users/GetAll',
          dataType: 'json',
          delay: 250,
          data: function (params) {
            return {
              NameFilter: params.term, // search term
              SkipCount: (params.page || 0) * 10,
              MaxResultCount: 10,
            };
          },
          processResults: function (data, params) {
            params.page = params.page || 0;

            return {
              results: $.map(data.result.items, function (item) {
                return {
                  text: item.user.name,
                  id: item.user.id,
                };
              }),
              pagination: {
                more: params.page * 10 < data.result.totalCount,
              },
            };
          },
          cache: true,
        },
      });

      var selectedEntityId = $('#WareHouseGift_UserId');

      if (selectedEntityId && selectedEntityId.val()) {
        abp
          .ajax({
            type: 'GET',
              url: '/api/services/app/User/GetUserForView',
              data: {
              id: selectedEntityId.val(),
            },
          })
          .done(function (data) {
            var option = new Option(data.user.name, data.user.id, true, true);
            typeaheadUserId.append(option).trigger('change');
          });
      }
      // Init select2
      var typeaheadProductPromotionId = $('#productPromotionId-typeahead-selector');

      typeaheadProductPromotionId.select2({
        placeholder: 'Select',
        theme: 'bootstrap5',
        selectionCssClass: 'form-select',
        dropdownParent: _modalManager.getModal(),
        minimumInputLength: 2,
        ajax: {
          url: abp.appPath + 'api/services/app/ProductPromotions/GetAll',
          dataType: 'json',
          delay: 250,
          data: function (params) {
            return {
              PromotionCodeFilter: params.term, // search term
              SkipCount: (params.page || 0) * 10,
              MaxResultCount: 10,
            };
          },
          processResults: function (data, params) {
            params.page = params.page || 0;

            return {
              results: $.map(data.result.items, function (item) {
                return {
                  text: item.productPromotion.name,
                  id: item.productPromotion.id,
                };
              }),
              pagination: {
                more: params.page * 10 < data.result.totalCount,
              },
            };
          },
          cache: true,
        },
      });

      var selectedEntityId = $('#WareHouseGift_ProductPromotionId');

      if (selectedEntityId && selectedEntityId.val()) {
        abp
          .ajax({
            type: 'GET',
            url: '/api/services/app/ProductPromotions/GetProductPromotionForView',
            data: {
              id: selectedEntityId.val(),
            },
          })
          .done(function (data) {
            var option = new Option(data.productPromotion.name, data.productPromotion.id, true, true);
            typeaheadProductPromotionId.append(option).trigger('change');
          });
      }

      _$wareHouseGiftInformationForm = _modalManager.getModal().find('form[name=WareHouseGiftInformationsForm]');
      _$wareHouseGiftInformationForm.validate();
    };

    $('#OpenUserLookupTableButton').click(function () {
      var wareHouseGift = _$wareHouseGiftInformationForm.serializeFormToObject();

      _WareHouseGiftuserLookupTableModal.open(
        { id: wareHouseGift.userId, displayName: wareHouseGift.userName },
        function (data) {
          _$wareHouseGiftInformationForm.find('input[name=userName]').val(data.displayName);
          _$wareHouseGiftInformationForm.find('input[name=userId]').val(data.id);
        },
      );
    });

    $('#ClearUserNameButton').click(function () {
      _$wareHouseGiftInformationForm.find('input[name=userName]').val('');
      _$wareHouseGiftInformationForm.find('input[name=userId]').val('');
    });

    $('#OpenProductPromotionLookupTableButton').click(function () {
      var wareHouseGift = _$wareHouseGiftInformationForm.serializeFormToObject();

      _WareHouseGiftproductPromotionLookupTableModal.open(
        { id: wareHouseGift.productPromotionId, displayName: wareHouseGift.productPromotionPromotionCode },
        function (data) {
          _$wareHouseGiftInformationForm.find('input[name=productPromotionPromotionCode]').val(data.displayName);
          _$wareHouseGiftInformationForm.find('input[name=productPromotionId]').val(data.id);
        },
      );
    });

    $('#ClearProductPromotionPromotionCodeButton').click(function () {
      _$wareHouseGiftInformationForm.find('input[name=productPromotionPromotionCode]').val('');
      _$wareHouseGiftInformationForm.find('input[name=productPromotionId]').val('');
    });

    this.save = function () {
      if (!_$wareHouseGiftInformationForm.valid()) {
        return;
      }
      if ($('#WareHouseGift_UserId').prop('required') && $('#WareHouseGift_UserId').val() == '') {
        abp.message.error(app.localize('{0}IsRequired', app.localize('User')));
        return;
      }
      if (
        $('#WareHouseGift_ProductPromotionId').prop('required') &&
        $('#WareHouseGift_ProductPromotionId').val() == ''
      ) {
        abp.message.error(app.localize('{0}IsRequired', app.localize('ProductPromotion')));
        return;
      }

      var wareHouseGift = _$wareHouseGiftInformationForm.serializeFormToObject();

      _modalManager.setBusy(true);
      _wareHouseGiftsService
        .createOrEdit(wareHouseGift)
        .done(function () {
          abp.notify.info(app.localize('SavedSuccessfully'));
          _modalManager.close();
          abp.event.trigger('app.createOrEditWareHouseGiftModalSaved');
        })
        .always(function () {
          _modalManager.setBusy(false);
        });
    };
  };
})(jQuery);
