﻿(function ($) {
  app.modals.CreateOrEditRankLevelModal = function () {
    var _rankLevelsService = abp.services.app.rankLevels;

    var _modalManager;
    var _$rankLevelInformationForm = null;

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

      _$rankLevelInformationForm = _modalManager.getModal().find('form[name=RankLevelInformationsForm]');
      _$rankLevelInformationForm.validate();
    };

    this.save = function () {
      if (!_$rankLevelInformationForm.valid()) {
        return;
      }

      if (_fileUploading != null && _fileUploading.length > 0) {
        abp.notify.info(app.localize('WaitingForFileUpload'));
        return;
      }

      var rankLevel = _$rankLevelInformationForm.serializeFormToObject();

      rankLevel.logoToken = _logoToken;

      _modalManager.setBusy(true);
      _rankLevelsService
        .createOrEdit(rankLevel)
        .done(function () {
          abp.notify.info(app.localize('SavedSuccessfully'));
          _modalManager.close();
          abp.event.trigger('app.createOrEditRankLevelModalSaved');
        })
        .always(function () {
          _modalManager.setBusy(false);
        });
    };

    $('#RankLevel_Logo').change(function () {
      var file = $(this)[0].files[0];
      if (!file) {
        _logoToken = null;
        return;
      }

      var formData = new FormData();
      formData.append('file', file);
      _fileUploading.push(true);

      $.ajax({
        url: '/App/RankLevels/UploadLogoFile',
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

    $('#RankLevel_Logo_Remove').click(function () {
      abp.message.confirm(app.localize('DoYouWantToRemoveTheFile'), app.localize('AreYouSure'), function (isConfirmed) {
        if (isConfirmed) {
          var RankLevel = _$rankLevelInformationForm.serializeFormToObject();
          _rankLevelsService
            .removeLogoFile({
              id: RankLevel.id,
            })
            .done(function () {
              abp.notify.success(app.localize('SuccessfullyDeleted'));
              _$rankLevelInformationForm.find('#div_current_file').css('display', 'none');
            });
        }
      });
    });

    $('#RankLevel_Logo').change(function () {
      var fileName = app.localize('ChooseAFile');
      if (this.files && this.files[0]) {
        fileName = this.files[0].name;
      }
      $('#RankLevel_LogoLabel').text(fileName);
    });
  };
})(jQuery);
