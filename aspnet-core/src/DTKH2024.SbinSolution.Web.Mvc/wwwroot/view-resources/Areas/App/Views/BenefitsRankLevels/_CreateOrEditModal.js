(function ($) {
  app.modals.CreateOrEditBenefitsRankLevelModal = function () {
    var _benefitsRankLevelsService = abp.services.app.benefitsRankLevels;

    var _modalManager;
    var _$benefitsRankLevelInformationForm = null;

    var _BenefitsRankLevelrankLevelLookupTableModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/BenefitsRankLevels/RankLevelLookupTableModal',
      scriptUrl:
        abp.appPath +
        'view-resources/Areas/App/Views/BenefitsRankLevels/_BenefitsRankLevelRankLevelLookupTableModal.js',
      modalClass: 'RankLevelLookupTableModal',
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
      var typeaheadRankLevelId = $('#rankLevelId-typeahead-selector');

      typeaheadRankLevelId.select2({
        placeholder: 'Select',
        theme: 'bootstrap5',
        selectionCssClass: 'form-select',
        dropdownParent: _modalManager.getModal(),
        minimumInputLength: 2,
        ajax: {
          url: abp.appPath + 'api/services/app/RankLevels/GetAll',
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
                  text: item.rankLevel.name,
                  id: item.rankLevel.id,
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

      var selectedEntityId = $('#BenefitsRankLevel_RankLevelId');

      if (selectedEntityId && selectedEntityId.val()) {
        abp
          .ajax({
            type: 'GET',
            url: '/api/services/app/RankLevels/GetRankLevelForView',
            data: {
              id: selectedEntityId.val(),
            },
          })
          .done(function (data) {
            var option = new Option(data.rankLevel.name, data.rankLevel.id, true, true);
            typeaheadRankLevelId.append(option).trigger('change');
          });
      }

      _$benefitsRankLevelInformationForm = _modalManager
        .getModal()
        .find('form[name=BenefitsRankLevelInformationsForm]');
      _$benefitsRankLevelInformationForm.validate();
    };

    $('#OpenRankLevelLookupTableButton').click(function () {
      var benefitsRankLevel = _$benefitsRankLevelInformationForm.serializeFormToObject();

      _BenefitsRankLevelrankLevelLookupTableModal.open(
        { id: benefitsRankLevel.rankLevelId, displayName: benefitsRankLevel.rankLevelName },
        function (data) {
          _$benefitsRankLevelInformationForm.find('input[name=rankLevelName]').val(data.displayName);
          _$benefitsRankLevelInformationForm.find('input[name=rankLevelId]').val(data.id);
        },
      );
    });

    $('#ClearRankLevelNameButton').click(function () {
      _$benefitsRankLevelInformationForm.find('input[name=rankLevelName]').val('');
      _$benefitsRankLevelInformationForm.find('input[name=rankLevelId]').val('');
    });

    this.save = function () {
      if (!_$benefitsRankLevelInformationForm.valid()) {
        return;
      }
      if ($('#BenefitsRankLevel_RankLevelId').prop('required') && $('#BenefitsRankLevel_RankLevelId').val() == '') {
        abp.message.error(app.localize('{0}IsRequired', app.localize('RankLevel')));
        return;
      }

      var benefitsRankLevel = _$benefitsRankLevelInformationForm.serializeFormToObject();

      _modalManager.setBusy(true);
      _benefitsRankLevelsService
        .createOrEdit(benefitsRankLevel)
        .done(function () {
          abp.notify.info(app.localize('SavedSuccessfully'));
          _modalManager.close();
          abp.event.trigger('app.createOrEditBenefitsRankLevelModalSaved');
        })
        .always(function () {
          _modalManager.setBusy(false);
        });
    };
  };
})(jQuery);
