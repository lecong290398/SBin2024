﻿(function ($) {
  app.modals.CreateOrEditBrandModal = function () {
    var _brandsService = abp.services.app.brands;

    var _modalManager;
    var _$brandInformationForm = null;

    var _fileUploading = [];
    var _logoToken;

    this.init = function (modalManager) {
      _modalManager = modalManager;

      var modal = _modalManager.getModal();
      modal.find('.date-picker').daterangepicker({
        singleDatePicker: true,
        locale: abp.localization.currentLanguage.name,
        format: 'L',
      });

      _$brandInformationForm = _modalManager.getModal().find('form[name=BrandInformationsForm]');
      _$brandInformationForm.validate();
    };

    this.save = function () {
      if (!_$brandInformationForm.valid()) {
        return;
      }

      if (_fileUploading != null && _fileUploading.length > 0) {
        abp.notify.info(app.localize('WaitingForFileUpload'));
        return;
      }

      var brand = _$brandInformationForm.serializeFormToObject();

      brand.logoToken = _logoToken;

      _modalManager.setBusy(true);
      _brandsService
        .createOrEdit(brand)
        .done(function () {
          abp.notify.info(app.localize('SavedSuccessfully'));
          _modalManager.close();
          abp.event.trigger('app.createOrEditBrandModalSaved');
        })
        .always(function () {
          _modalManager.setBusy(false);
        });
    };

    $('#Brand_Logo').change(function () {
      var file = $(this)[0].files[0];
      if (!file) {
        _logoToken = null;
        return;
      }

      var formData = new FormData();
      formData.append('file', file);
      _fileUploading.push(true);

      $.ajax({
        url: '/App/Brands/UploadLogoFile',
        type: 'POST',
        data: formData,
        processData: false,
        contentType: false,
      })
        .done(function (resp) {
          if (resp.success && resp.result.fileToken) {
            _logoToken = resp.result.fileToken;
          } else {
            abp.message.error(resp.result.message);
          }
        })
        .always(function () {
          _fileUploading.pop();
        });
    });

    $('#Brand_Logo_Remove').click(function () {
      abp.message.confirm(app.localize('DoYouWantToRemoveTheFile'), app.localize('AreYouSure'), function (isConfirmed) {
        if (isConfirmed) {
          var Brand = _$brandInformationForm.serializeFormToObject();
          _brandsService
            .removeLogoFile({
              id: Brand.id,
            })
            .done(function () {
              abp.notify.success(app.localize('SuccessfullyDeleted'));
              _$brandInformationForm.find('#div_current_file').css('display', 'none');
            });
        }
      });
    });

    $('#Brand_Logo').change(function () {
      var fileName = app.localize('ChooseAFile');
      if (this.files && this.files[0]) {
        fileName = this.files[0].name;
      }
      $('#Brand_LogoLabel').text(fileName);
    });
  };
})(jQuery);
