﻿(function ($) {
  app.modals.CreateOrEditTransactionStatusModal = function () {
    var _transactionStatusesService = abp.services.app.transactionStatuses;

    var _modalManager;
    var _$transactionStatusInformationForm = null;

    this.init = function (modalManager) {
      _modalManager = modalManager;

      var modal = _modalManager.getModal();
      modal.find('.date-picker').daterangepicker({
        singleDatePicker: true,
        locale: abp.localization.currentLanguage.name,
        format: 'L',
      });

      _$transactionStatusInformationForm = _modalManager
        .getModal()
        .find('form[name=TransactionStatusInformationsForm]');
      _$transactionStatusInformationForm.validate();
    };

    this.save = function () {
      if (!_$transactionStatusInformationForm.valid()) {
        return;
      }

      var transactionStatus = _$transactionStatusInformationForm.serializeFormToObject();

      _modalManager.setBusy(true);
      _transactionStatusesService
        .createOrEdit(transactionStatus)
        .done(function () {
          abp.notify.info(app.localize('SavedSuccessfully'));
          _modalManager.close();
          abp.event.trigger('app.createOrEditTransactionStatusModalSaved');
        })
        .always(function () {
          _modalManager.setBusy(false);
        });
    };
  };
})(jQuery);
