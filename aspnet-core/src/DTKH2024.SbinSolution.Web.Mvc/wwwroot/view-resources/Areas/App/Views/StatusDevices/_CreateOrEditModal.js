﻿(function ($) {
  app.modals.CreateOrEditStatusDeviceModal = function () {
    var _statusDevicesService = abp.services.app.statusDevices;

    var _modalManager;
    var _$statusDeviceInformationForm = null;

    this.init = function (modalManager) {
      _modalManager = modalManager;

      var modal = _modalManager.getModal();
      modal.find('.date-picker').daterangepicker({
        singleDatePicker: true,
        locale: abp.localization.currentLanguage.name,
        format: 'L',
      });

      _$statusDeviceInformationForm = _modalManager.getModal().find('form[name=StatusDeviceInformationsForm]');
      _$statusDeviceInformationForm.validate();
    };

    this.save = function () {
      if (!_$statusDeviceInformationForm.valid()) {
        return;
      }

      var statusDevice = _$statusDeviceInformationForm.serializeFormToObject();

      _modalManager.setBusy(true);
      _statusDevicesService
        .createOrEdit(statusDevice)
        .done(function () {
          abp.notify.info(app.localize('SavedSuccessfully'));
          _modalManager.close();
          abp.event.trigger('app.createOrEditStatusDeviceModalSaved');
        })
        .always(function () {
          _modalManager.setBusy(false);
        });
    };
  };
})(jQuery);
