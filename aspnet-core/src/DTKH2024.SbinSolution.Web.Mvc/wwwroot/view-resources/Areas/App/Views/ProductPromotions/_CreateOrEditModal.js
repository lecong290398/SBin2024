﻿(function ($) {
  app.modals.CreateOrEditProductPromotionModal = function () {
    var _productPromotionsService = abp.services.app.productPromotions;

    var _modalManager;
    var _$productPromotionInformationForm = null;

    var _ProductPromotionproductLookupTableModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/ProductPromotions/ProductLookupTableModal',
      scriptUrl:
        abp.appPath + 'view-resources/Areas/App/Views/ProductPromotions/_ProductPromotionProductLookupTableModal.js',
      modalClass: 'ProductLookupTableModal',
    });
    var _ProductPromotioncategoryPromotionLookupTableModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/ProductPromotions/CategoryPromotionLookupTableModal',
      scriptUrl:
        abp.appPath +
        'view-resources/Areas/App/Views/ProductPromotions/_ProductPromotionCategoryPromotionLookupTableModal.js',
      modalClass: 'CategoryPromotionLookupTableModal',
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
      var typeaheadProductId = $('#productId-typeahead-selector');

      typeaheadProductId.select2({
        placeholder: 'Select',
        theme: 'bootstrap5',
        selectionCssClass: 'form-select',
        dropdownParent: _modalManager.getModal(),
        minimumInputLength: 2,
        ajax: {
          url: abp.appPath + 'api/services/app/Products/GetAll',
          dataType: 'json',
          delay: 250,
          data: function (params) {
            return {
              ProductNameFilter: params.term, // search term
              SkipCount: (params.page || 0) * 10,
              MaxResultCount: 10,
            };
          },
          processResults: function (data, params) {
            params.page = params.page || 0;

            return {
              results: $.map(data.result.items, function (item) {
                return {
                  text: item.product.name,
                  id: item.product.id,
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

      var selectedEntityId = $('#ProductPromotion_ProductId');

      if (selectedEntityId && selectedEntityId.val()) {
        abp
          .ajax({
            type: 'GET',
            url: '/api/services/app/Products/GetProductForView',
            data: {
              id: selectedEntityId.val(),
            },
          })
          .done(function (data) {
            var option = new Option(data.product.name, data.product.id, true, true);
            typeaheadProductId.append(option).trigger('change');
          });
      }
      // Init select2
      var typeaheadCategoryPromotionId = $('#categoryPromotionId-typeahead-selector');

      typeaheadCategoryPromotionId.select2({
        placeholder: 'Select',
        theme: 'bootstrap5',
        selectionCssClass: 'form-select',
        dropdownParent: _modalManager.getModal(),
        minimumInputLength: 2,
        ajax: {
          url: abp.appPath + 'api/services/app/CategoryPromotions/GetAll',
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
                  text: item.categoryPromotion.name,
                  id: item.categoryPromotion.id,
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

      var selectedEntityId = $('#ProductPromotion_CategoryPromotionId');

      if (selectedEntityId && selectedEntityId.val()) {
        abp
          .ajax({
            type: 'GET',
            url: '/api/services/app/CategoryPromotions/GetCategoryPromotionForView',
            data: {
              id: selectedEntityId.val(),
            },
          })
          .done(function (data) {
            var option = new Option(data.categoryPromotion.name, data.categoryPromotion.id, true, true);
            typeaheadCategoryPromotionId.append(option).trigger('change');
          });
      }

      _$productPromotionInformationForm = _modalManager.getModal().find('form[name=ProductPromotionInformationsForm]');
      _$productPromotionInformationForm.validate();
    };

    $('#OpenProductLookupTableButton').click(function () {
      var productPromotion = _$productPromotionInformationForm.serializeFormToObject();

      _ProductPromotionproductLookupTableModal.open(
        { id: productPromotion.productId, displayName: productPromotion.productProductName },
        function (data) {
          _$productPromotionInformationForm.find('input[name=productProductName]').val(data.displayName);
          _$productPromotionInformationForm.find('input[name=productId]').val(data.id);
        },
      );
    });

    $('#ClearProductProductNameButton').click(function () {
      _$productPromotionInformationForm.find('input[name=productProductName]').val('');
      _$productPromotionInformationForm.find('input[name=productId]').val('');
    });

    $('#OpenCategoryPromotionLookupTableButton').click(function () {
      var productPromotion = _$productPromotionInformationForm.serializeFormToObject();

      _ProductPromotioncategoryPromotionLookupTableModal.open(
        { id: productPromotion.categoryPromotionId, displayName: productPromotion.categoryPromotionName },
        function (data) {
          _$productPromotionInformationForm.find('input[name=categoryPromotionName]').val(data.displayName);
          _$productPromotionInformationForm.find('input[name=categoryPromotionId]').val(data.id);
        },
      );
    });

    $('#ClearCategoryPromotionNameButton').click(function () {
      _$productPromotionInformationForm.find('input[name=categoryPromotionName]').val('');
      _$productPromotionInformationForm.find('input[name=categoryPromotionId]').val('');
    });

    this.save = function () {
      if (!_$productPromotionInformationForm.valid()) {
        return;
      }
      if ($('#ProductPromotion_ProductId').prop('required') && $('#ProductPromotion_ProductId').val() == '') {
        abp.message.error(app.localize('{0}IsRequired', app.localize('Product')));
        return;
      }
      if (
        $('#ProductPromotion_CategoryPromotionId').prop('required') &&
        $('#ProductPromotion_CategoryPromotionId').val() == ''
      ) {
        abp.message.error(app.localize('{0}IsRequired', app.localize('CategoryPromotion')));
        return;
      }

      var productPromotion = _$productPromotionInformationForm.serializeFormToObject();

      _modalManager.setBusy(true);
      _productPromotionsService
        .createOrEdit(productPromotion)
        .done(function () {
          abp.notify.info(app.localize('SavedSuccessfully'));
          _modalManager.close();
          abp.event.trigger('app.createOrEditProductPromotionModalSaved');
        })
        .always(function () {
          _modalManager.setBusy(false);
        });
    };
  };
})(jQuery);
