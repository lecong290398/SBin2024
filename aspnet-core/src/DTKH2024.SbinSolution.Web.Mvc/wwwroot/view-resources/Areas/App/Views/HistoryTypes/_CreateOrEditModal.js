﻿(function ($) {
  app.modals.CreateOrEditHistoryTypeModal = function () {
    var _historyTypesService = abp.services.app.historyTypes;

    var _modalManager;
    var _$historyTypeInformationForm = null;

    this.init = function (modalManager) {
      _modalManager = modalManager;

      var modal = _modalManager.getModal();
      modal.find('.date-picker').daterangepicker({
        singleDatePicker: true,
        locale: abp.localization.currentLanguage.name,
        format: 'L',
      });

      _$historyTypeInformationForm = _modalManager.getModal().find('form[name=HistoryTypeInformationsForm]');
      _$historyTypeInformationForm.validate();
    };

    this.save = function () {
      if (!_$historyTypeInformationForm.valid()) {
        return;
      }

      var historyType = _$historyTypeInformationForm.serializeFormToObject();

      _modalManager.setBusy(true);
      _historyTypesService
        .createOrEdit(historyType)
        .done(function () {
          abp.notify.info(app.localize('SavedSuccessfully'));
          _modalManager.close();
          abp.event.trigger('app.createOrEditHistoryTypeModalSaved');
        })
        .always(function () {
          _modalManager.setBusy(false);
        });
    };
  };
})(jQuery);
