﻿(function ($) {
  app.modals.CreateOrEditProductTypeModal = function () {
    var _productTypesService = abp.services.app.productTypes;

    var _modalManager;
    var _$productTypeInformationForm = null;

    this.init = function (modalManager) {
      _modalManager = modalManager;

      var modal = _modalManager.getModal();
      modal.find('.date-picker').daterangepicker({
        singleDatePicker: true,
        locale: abp.localization.currentLanguage.name,
        format: 'L',
      });

      _$productTypeInformationForm = _modalManager.getModal().find('form[name=ProductTypeInformationsForm]');
      _$productTypeInformationForm.validate();
    };

    this.save = function () {
      if (!_$productTypeInformationForm.valid()) {
        return;
      }

      var productType = _$productTypeInformationForm.serializeFormToObject();

      _modalManager.setBusy(true);
      _productTypesService
        .createOrEdit(productType)
        .done(function () {
          abp.notify.info(app.localize('SavedSuccessfully'));
          _modalManager.close();
          abp.event.trigger('app.createOrEditProductTypeModalSaved');
        })
        .always(function () {
          _modalManager.setBusy(false);
        });
    };
  };
})(jQuery);
