﻿(function ($) {
  app.modals.CreateOrEditTransactionBinModal = function () {
    var _transactionBinsService = abp.services.app.transactionBins;

    var _modalManager;
    var _$transactionBinInformationForm = null;

    var _TransactionBindeviceLookupTableModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/TransactionBins/DeviceLookupTableModal',
      scriptUrl:
        abp.appPath + 'view-resources/Areas/App/Views/TransactionBins/_TransactionBinDeviceLookupTableModal.js',
      modalClass: 'DeviceLookupTableModal',
    });
    var _TransactionBinuserLookupTableModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/TransactionBins/UserLookupTableModal',
      scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/TransactionBins/_TransactionBinUserLookupTableModal.js',
      modalClass: 'UserLookupTableModal',
    });
    var _TransactionBintransactionStatusLookupTableModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/TransactionBins/TransactionStatusLookupTableModal',
      scriptUrl:
        abp.appPath +
        'view-resources/Areas/App/Views/TransactionBins/_TransactionBinTransactionStatusLookupTableModal.js',
      modalClass: 'TransactionStatusLookupTableModal',
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
      var typeaheadDeviceId = $('#deviceId-typeahead-selector');

      typeaheadDeviceId.select2({
        placeholder: 'Select',
        theme: 'bootstrap5',
        selectionCssClass: 'form-select',
        dropdownParent: _modalManager.getModal(),
        minimumInputLength: 2,
        ajax: {
          url: abp.appPath + 'api/services/app/Devices/GetAll',
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
                  text: item.device.name,
                  id: item.device.id,
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

      var selectedEntityId = $('#TransactionBin_DeviceId');

      if (selectedEntityId && selectedEntityId.val()) {
        abp
          .ajax({
            type: 'GET',
            url: '/api/services/app/Devices/GetDeviceForView',
            data: {
              id: selectedEntityId.val(),
            },
          })
          .done(function (data) {
            var option = new Option(data.device.name, data.device.id, true, true);
            typeaheadDeviceId.append(option).trigger('change');
          });
      }
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

      var selectedEntityId = $('#TransactionBin_UserId');

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
      var typeaheadTransactionStatusId = $('#transactionStatusId-typeahead-selector');

      typeaheadTransactionStatusId.select2({
        placeholder: 'Select',
        theme: 'bootstrap5',
        selectionCssClass: 'form-select',
        dropdownParent: _modalManager.getModal(),
        minimumInputLength: 2,
        ajax: {
          url: abp.appPath + 'api/services/app/TransactionStatuses/GetAll',
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
                  text: item.transactionStatus.name,
                  id: item.transactionStatus.id,
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

      var selectedEntityId = $('#TransactionBin_TransactionStatusId');

      if (selectedEntityId && selectedEntityId.val()) {
        abp
          .ajax({
            type: 'GET',
            url: '/api/services/app/TransactionStatuses/GetTransactionStatusForView',
            data: {
              id: selectedEntityId.val(),
            },
          })
          .done(function (data) {
            var option = new Option(data.transactionStatus.name, data.transactionStatus.id, true, true);
            typeaheadTransactionStatusId.append(option).trigger('change');
          });
      }

      _$transactionBinInformationForm = _modalManager.getModal().find('form[name=TransactionBinInformationsForm]');
      _$transactionBinInformationForm.validate();
    };

    $('#OpenDeviceLookupTableButton').click(function () {
      var transactionBin = _$transactionBinInformationForm.serializeFormToObject();

      _TransactionBindeviceLookupTableModal.open(
        { id: transactionBin.deviceId, displayName: transactionBin.deviceName },
        function (data) {
          _$transactionBinInformationForm.find('input[name=deviceName]').val(data.displayName);
          _$transactionBinInformationForm.find('input[name=deviceId]').val(data.id);
        },
      );
    });

    $('#ClearDeviceNameButton').click(function () {
      _$transactionBinInformationForm.find('input[name=deviceName]').val('');
      _$transactionBinInformationForm.find('input[name=deviceId]').val('');
    });

    $('#OpenUserLookupTableButton').click(function () {
      var transactionBin = _$transactionBinInformationForm.serializeFormToObject();

      _TransactionBinuserLookupTableModal.open(
        { id: transactionBin.userId, displayName: transactionBin.userName },
        function (data) {
          _$transactionBinInformationForm.find('input[name=userName]').val(data.displayName);
          _$transactionBinInformationForm.find('input[name=userId]').val(data.id);
        },
      );
    });

    $('#ClearUserNameButton').click(function () {
      _$transactionBinInformationForm.find('input[name=userName]').val('');
      _$transactionBinInformationForm.find('input[name=userId]').val('');
    });

    $('#OpenTransactionStatusLookupTableButton').click(function () {
      var transactionBin = _$transactionBinInformationForm.serializeFormToObject();

      _TransactionBintransactionStatusLookupTableModal.open(
        { id: transactionBin.transactionStatusId, displayName: transactionBin.transactionStatusName },
        function (data) {
          _$transactionBinInformationForm.find('input[name=transactionStatusName]').val(data.displayName);
          _$transactionBinInformationForm.find('input[name=transactionStatusId]').val(data.id);
        },
      );
    });

    $('#ClearTransactionStatusNameButton').click(function () {
      _$transactionBinInformationForm.find('input[name=transactionStatusName]').val('');
      _$transactionBinInformationForm.find('input[name=transactionStatusId]').val('');
    });

    this.save = function () {
      if (!_$transactionBinInformationForm.valid()) {
        return;
      }
      if ($('#TransactionBin_DeviceId').prop('required') && $('#TransactionBin_DeviceId').val() == '') {
        abp.message.error(app.localize('{0}IsRequired', app.localize('Device')));
        return;
      }
      if ($('#TransactionBin_UserId').prop('required') && $('#TransactionBin_UserId').val() == '') {
        abp.message.error(app.localize('{0}IsRequired', app.localize('User')));
        return;
      }
      if (
        $('#TransactionBin_TransactionStatusId').prop('required') &&
        $('#TransactionBin_TransactionStatusId').val() == ''
      ) {
        abp.message.error(app.localize('{0}IsRequired', app.localize('TransactionStatus')));
        return;
      }

      var transactionBin = _$transactionBinInformationForm.serializeFormToObject();

      _modalManager.setBusy(true);
      _transactionBinsService
        .createOrEdit(transactionBin)
        .done(function () {
          abp.notify.info(app.localize('SavedSuccessfully'));
          _modalManager.close();
          abp.event.trigger('app.createOrEditTransactionBinModalSaved');
        })
        .always(function () {
          _modalManager.setBusy(false);
        });
    };
  };
})(jQuery);
