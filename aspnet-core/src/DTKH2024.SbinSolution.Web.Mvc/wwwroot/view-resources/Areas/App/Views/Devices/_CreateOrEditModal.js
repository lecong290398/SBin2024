(function ($) {
  app.modals.CreateOrEditDeviceModal = function () {
    var _devicesService = abp.services.app.devices;

    var _modalManager;
    var _$deviceInformationForm = null;

    var _DevicestatusDeviceLookupTableModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/Devices/StatusDeviceLookupTableModal',
      scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/Devices/_DeviceStatusDeviceLookupTableModal.js',
      modalClass: 'StatusDeviceLookupTableModal',
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
      var typeaheadStatusDeviceId = $('#statusDeviceId-typeahead-selector');

      typeaheadStatusDeviceId.select2({
        placeholder: 'Select',
        theme: 'bootstrap5',
        selectionCssClass: 'form-select',
        dropdownParent: _modalManager.getModal(),
        minimumInputLength: 2,
        ajax: {
          url: abp.appPath + 'api/services/app/StatusDevices/GetAll',
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
                  text: item.statusDevice.name,
                  id: item.statusDevice.id,
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

      var selectedEntityId = $('#Device_StatusDeviceId');

      if (selectedEntityId && selectedEntityId.val()) {
        abp
          .ajax({
            type: 'GET',
            url: '/api/services/app/StatusDevices/GetStatusDeviceForView',
            data: {
              id: selectedEntityId.val(),
            },
          })
          .done(function (data) {
            var option = new Option(data.statusDevice.name, data.statusDevice.id, true, true);
            typeaheadStatusDeviceId.append(option).trigger('change');
          });
      }

      _$deviceInformationForm = _modalManager.getModal().find('form[name=DeviceInformationsForm]');
      _$deviceInformationForm.validate();
    };

    $('#OpenStatusDeviceLookupTableButton').click(function () {
      var device = _$deviceInformationForm.serializeFormToObject();

      _DevicestatusDeviceLookupTableModal.open(
        { id: device.statusDeviceId, displayName: device.statusDeviceName },
        function (data) {
          _$deviceInformationForm.find('input[name=statusDeviceName]').val(data.displayName);
          _$deviceInformationForm.find('input[name=statusDeviceId]').val(data.id);
        },
      );
    });

    $('#ClearStatusDeviceNameButton').click(function () {
      _$deviceInformationForm.find('input[name=statusDeviceName]').val('');
      _$deviceInformationForm.find('input[name=statusDeviceId]').val('');
    });

    this.save = function () {
      if (!_$deviceInformationForm.valid()) {
        return;
      }
      if ($('#Device_StatusDeviceId').prop('required') && $('#Device_StatusDeviceId').val() == '') {
        abp.message.error(app.localize('{0}IsRequired', app.localize('StatusDevice')));
        return;
      }

      var device = _$deviceInformationForm.serializeFormToObject();

      _modalManager.setBusy(true);
      _devicesService
        .createOrEdit(device)
        .done(function () {
          abp.notify.info(app.localize('SavedSuccessfully'));
          _modalManager.close();
          abp.event.trigger('app.createOrEditDeviceModalSaved');
        })
        .always(function () {
          _modalManager.setBusy(false);
        });
    };
  };
})(jQuery);
