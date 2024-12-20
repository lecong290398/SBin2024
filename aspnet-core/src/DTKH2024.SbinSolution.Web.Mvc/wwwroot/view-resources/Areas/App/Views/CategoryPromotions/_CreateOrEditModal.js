﻿(function ($) {
  app.modals.CreateOrEditCategoryPromotionModal = function () {
    var _categoryPromotionsService = abp.services.app.categoryPromotions;

    var _modalManager;
    var _$categoryPromotionInformationForm = null;

    this.init = function (modalManager) {
      _modalManager = modalManager;

      var modal = _modalManager.getModal();
      modal.find('.date-picker').daterangepicker({
        singleDatePicker: true,
        locale: abp.localization.currentLanguage.name,
        format: 'L',
      });

      _$categoryPromotionInformationForm = _modalManager
        .getModal()
        .find('form[name=CategoryPromotionInformationsForm]');
      _$categoryPromotionInformationForm.validate();
    };

    this.save = function () {
      if (!_$categoryPromotionInformationForm.valid()) {
        return;
      }

      var categoryPromotion = _$categoryPromotionInformationForm.serializeFormToObject();

      _modalManager.setBusy(true);
      _categoryPromotionsService
        .createOrEdit(categoryPromotion)
        .done(function () {
          abp.notify.info(app.localize('SavedSuccessfully'));
          _modalManager.close();
          abp.event.trigger('app.createOrEditCategoryPromotionModalSaved');
        })
        .always(function () {
          _modalManager.setBusy(false);
        });
    };
  };
})(jQuery);
