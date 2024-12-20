﻿(function ($) {
  app.modals.CreateOrEditOrderHistoryModal = function () {
    var _orderHistoriesService = abp.services.app.orderHistories;

    var _modalManager;
    var _$orderHistoryInformationForm = null;

    var _OrderHistoryuserLookupTableModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/OrderHistories/UserLookupTableModal',
      scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/OrderHistories/_OrderHistoryUserLookupTableModal.js',
      modalClass: 'UserLookupTableModal',
    });
    var _OrderHistorytransactionBinLookupTableModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/OrderHistories/TransactionBinLookupTableModal',
      scriptUrl:
        abp.appPath + 'view-resources/Areas/App/Views/OrderHistories/_OrderHistoryTransactionBinLookupTableModal.js',
      modalClass: 'TransactionBinLookupTableModal',
    });
    var _OrderHistorywareHouseGiftLookupTableModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/OrderHistories/WareHouseGiftLookupTableModal',
      scriptUrl:
        abp.appPath + 'view-resources/Areas/App/Views/OrderHistories/_OrderHistoryWareHouseGiftLookupTableModal.js',
      modalClass: 'WareHouseGiftLookupTableModal',
    });
    var _OrderHistoryhistoryTypeLookupTableModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/OrderHistories/HistoryTypeLookupTableModal',
      scriptUrl:
        abp.appPath + 'view-resources/Areas/App/Views/OrderHistories/_OrderHistoryHistoryTypeLookupTableModal.js',
      modalClass: 'HistoryTypeLookupTableModal',
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

      var selectedEntityId = $('#OrderHistory_UserId');

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
      var typeaheadTransactionBinId = $('#transactionBinId-typeahead-selector');

      typeaheadTransactionBinId.select2({
        placeholder: 'Select',
        theme: 'bootstrap5',
        selectionCssClass: 'form-select',
        dropdownParent: _modalManager.getModal(),
        minimumInputLength: 2,
        ajax: {
          url: abp.appPath + 'api/services/app/TransactionBins/GetAll',
          dataType: 'json',
          delay: 250,
          data: function (params) {
            return {
              TransactionCodeFilter: params.term, // search term
              SkipCount: (params.page || 0) * 10,
              MaxResultCount: 10,
            };
          },
          processResults: function (data, params) {
            params.page = params.page || 0;

            return {
              results: $.map(data.result.items, function (item) {
                return {
                  text: item.transactionBin.name,
                  id: item.transactionBin.id,
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

      var selectedEntityId = $('#OrderHistory_TransactionBinId');

      if (selectedEntityId && selectedEntityId.val()) {
        abp
          .ajax({
            type: 'GET',
            url: '/api/services/app/TransactionBins/GetTransactionBinForView',
            data: {
              id: selectedEntityId.val(),
            },
          })
          .done(function (data) {
            var option = new Option(data.transactionBin.name, data.transactionBin.id, true, true);
            typeaheadTransactionBinId.append(option).trigger('change');
          });
      }
      // Init select2
      var typeaheadWareHouseGiftId = $('#wareHouseGiftId-typeahead-selector');

      typeaheadWareHouseGiftId.select2({
        placeholder: 'Select',
        theme: 'bootstrap5',
        selectionCssClass: 'form-select',
        dropdownParent: _modalManager.getModal(),
        minimumInputLength: 2,
        ajax: {
          url: abp.appPath + 'api/services/app/WareHouseGifts/GetAll',
          dataType: 'json',
          delay: 250,
          data: function (params) {
            return {
              CodeFilter: params.term, // search term
              SkipCount: (params.page || 0) * 10,
              MaxResultCount: 10,
            };
          },
          processResults: function (data, params) {
            params.page = params.page || 0;

            return {
              results: $.map(data.result.items, function (item) {
                return {
                  text: item.wareHouseGift.name,
                  id: item.wareHouseGift.id,
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

      var selectedEntityId = $('#OrderHistory_WareHouseGiftId');

      if (selectedEntityId && selectedEntityId.val()) {
        abp
          .ajax({
            type: 'GET',
            url: '/api/services/app/WareHouseGifts/GetWareHouseGiftForView',
            data: {
              id: selectedEntityId.val(),
            },
          })
          .done(function (data) {
            var option = new Option(data.wareHouseGift.name, data.wareHouseGift.id, true, true);
            typeaheadWareHouseGiftId.append(option).trigger('change');
          });
      }
      // Init select2
      var typeaheadHistoryTypeId = $('#historyTypeId-typeahead-selector');

      typeaheadHistoryTypeId.select2({
        placeholder: 'Select',
        theme: 'bootstrap5',
        selectionCssClass: 'form-select',
        dropdownParent: _modalManager.getModal(),
        minimumInputLength: 2,
        ajax: {
          url: abp.appPath + 'api/services/app/HistoryTypes/GetAll',
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
                  text: item.historyType.name,
                  id: item.historyType.id,
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

      var selectedEntityId = $('#OrderHistory_HistoryTypeId');

      if (selectedEntityId && selectedEntityId.val()) {
        abp
          .ajax({
            type: 'GET',
            url: '/api/services/app/HistoryTypes/GetHistoryTypeForView',
            data: {
              id: selectedEntityId.val(),
            },
          })
          .done(function (data) {
            var option = new Option(data.historyType.name, data.historyType.id, true, true);
            typeaheadHistoryTypeId.append(option).trigger('change');
          });
      }

      _$orderHistoryInformationForm = _modalManager.getModal().find('form[name=OrderHistoryInformationsForm]');
      _$orderHistoryInformationForm.validate();
    };

    $('#OpenUserLookupTableButton').click(function () {
      var orderHistory = _$orderHistoryInformationForm.serializeFormToObject();

      _OrderHistoryuserLookupTableModal.open(
        { id: orderHistory.userId, displayName: orderHistory.userName },
        function (data) {
          _$orderHistoryInformationForm.find('input[name=userName]').val(data.displayName);
          _$orderHistoryInformationForm.find('input[name=userId]').val(data.id);
        },
      );
    });

    $('#ClearUserNameButton').click(function () {
      _$orderHistoryInformationForm.find('input[name=userName]').val('');
      _$orderHistoryInformationForm.find('input[name=userId]').val('');
    });

    $('#OpenTransactionBinLookupTableButton').click(function () {
      var orderHistory = _$orderHistoryInformationForm.serializeFormToObject();

      _OrderHistorytransactionBinLookupTableModal.open(
        { id: orderHistory.transactionBinId, displayName: orderHistory.transactionBinTransactionCode },
        function (data) {
          _$orderHistoryInformationForm.find('input[name=transactionBinTransactionCode]').val(data.displayName);
          _$orderHistoryInformationForm.find('input[name=transactionBinId]').val(data.id);
        },
      );
    });

    $('#ClearTransactionBinTransactionCodeButton').click(function () {
      _$orderHistoryInformationForm.find('input[name=transactionBinTransactionCode]').val('');
      _$orderHistoryInformationForm.find('input[name=transactionBinId]').val('');
    });

    $('#OpenWareHouseGiftLookupTableButton').click(function () {
      var orderHistory = _$orderHistoryInformationForm.serializeFormToObject();

      _OrderHistorywareHouseGiftLookupTableModal.open(
        { id: orderHistory.wareHouseGiftId, displayName: orderHistory.wareHouseGiftCode },
        function (data) {
          _$orderHistoryInformationForm.find('input[name=wareHouseGiftCode]').val(data.displayName);
          _$orderHistoryInformationForm.find('input[name=wareHouseGiftId]').val(data.id);
        },
      );
    });

    $('#ClearWareHouseGiftCodeButton').click(function () {
      _$orderHistoryInformationForm.find('input[name=wareHouseGiftCode]').val('');
      _$orderHistoryInformationForm.find('input[name=wareHouseGiftId]').val('');
    });

    $('#OpenHistoryTypeLookupTableButton').click(function () {
      var orderHistory = _$orderHistoryInformationForm.serializeFormToObject();

      _OrderHistoryhistoryTypeLookupTableModal.open(
        { id: orderHistory.historyTypeId, displayName: orderHistory.historyTypeName },
        function (data) {
          _$orderHistoryInformationForm.find('input[name=historyTypeName]').val(data.displayName);
          _$orderHistoryInformationForm.find('input[name=historyTypeId]').val(data.id);
        },
      );
    });

    $('#ClearHistoryTypeNameButton').click(function () {
      _$orderHistoryInformationForm.find('input[name=historyTypeName]').val('');
      _$orderHistoryInformationForm.find('input[name=historyTypeId]').val('');
    });

    this.save = function () {
      if (!_$orderHistoryInformationForm.valid()) {
        return;
      }
      if ($('#OrderHistory_UserId').prop('required') && $('#OrderHistory_UserId').val() == '') {
        abp.message.error(app.localize('{0}IsRequired', app.localize('User')));
        return;
      }
      if ($('#OrderHistory_TransactionBinId').prop('required') && $('#OrderHistory_TransactionBinId').val() == '') {
        abp.message.error(app.localize('{0}IsRequired', app.localize('TransactionBin')));
        return;
      }
      if ($('#OrderHistory_WareHouseGiftId').prop('required') && $('#OrderHistory_WareHouseGiftId').val() == '') {
        abp.message.error(app.localize('{0}IsRequired', app.localize('WareHouseGift')));
        return;
      }
      if ($('#OrderHistory_HistoryTypeId').prop('required') && $('#OrderHistory_HistoryTypeId').val() == '') {
        abp.message.error(app.localize('{0}IsRequired', app.localize('HistoryType')));
        return;
      }

      var orderHistory = _$orderHistoryInformationForm.serializeFormToObject();

      _modalManager.setBusy(true);
      _orderHistoriesService
        .createOrEdit(orderHistory)
        .done(function () {
          abp.notify.info(app.localize('SavedSuccessfully'));
          _modalManager.close();
          abp.event.trigger('app.createOrEditOrderHistoryModalSaved');
        })
        .always(function () {
          _modalManager.setBusy(false);
        });
    };
  };
})(jQuery);
