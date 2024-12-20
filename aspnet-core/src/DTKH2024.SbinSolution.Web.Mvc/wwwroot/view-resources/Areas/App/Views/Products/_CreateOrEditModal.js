﻿(function ($) {
  app.modals.CreateOrEditProductModal = function () {
    var _productsService = abp.services.app.products;

    var _modalManager;
    var _$productInformationForm = null;

    var _ProductproductTypeLookupTableModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/Products/ProductTypeLookupTableModal',
      scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/Products/_ProductProductTypeLookupTableModal.js',
      modalClass: 'ProductTypeLookupTableModal',
    });
    var _ProductbrandLookupTableModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/Products/BrandLookupTableModal',
      scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/Products/_ProductBrandLookupTableModal.js',
      modalClass: 'BrandLookupTableModal',
    });
    var _fileUploading = [];
    var _imageToken;

    this.init = function (modalManager) {
      _modalManager = modalManager;

      var modal = _modalManager.getModal();
      modal.find('.date-picker').daterangepicker({
        singleDatePicker: true,
        locale: abp.localization.currentLanguage.name,
        format: 'L',
      });

      // Init select2
      var typeaheadProductTypeId = $('#productTypeId-typeahead-selector');

      typeaheadProductTypeId.select2({
        placeholder: 'Select',
        theme: 'bootstrap5',
        selectionCssClass: 'form-select',
        dropdownParent: _modalManager.getModal(),
        minimumInputLength: 2,
        ajax: {
          url: abp.appPath + 'api/services/app/ProductTypes/GetAll',
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
                  text: item.productType.name,
                  id: item.productType.id,
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

      var selectedEntityId = $('#Product_ProductTypeId');

      if (selectedEntityId && selectedEntityId.val()) {
        abp
          .ajax({
            type: 'GET',
            url: '/api/services/app/ProductTypes/GetProductTypeForView',
            data: {
              id: selectedEntityId.val(),
            },
          })
          .done(function (data) {
            var option = new Option(data.productType.name, data.productType.id, true, true);
            typeaheadProductTypeId.append(option).trigger('change');
          });
      }
      // Init select2
      var typeaheadBrandId = $('#brandId-typeahead-selector');

      typeaheadBrandId.select2({
        placeholder: 'Select',
        theme: 'bootstrap5',
        selectionCssClass: 'form-select',
        dropdownParent: _modalManager.getModal(),
        minimumInputLength: 2,
        ajax: {
          url: abp.appPath + 'api/services/app/Brands/GetAll',
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
                  text: item.brand.name,
                  id: item.brand.id,
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

      var selectedEntityId = $('#Product_BrandId');

      if (selectedEntityId && selectedEntityId.val()) {
        abp
          .ajax({
            type: 'GET',
            url: '/api/services/app/Brands/GetBrandForView',
            data: {
              id: selectedEntityId.val(),
            },
          })
          .done(function (data) {
            var option = new Option(data.brand.name, data.brand.id, true, true);
            typeaheadBrandId.append(option).trigger('change');
          });
      }

      _$productInformationForm = _modalManager.getModal().find('form[name=ProductInformationsForm]');
      _$productInformationForm.validate();
    };

    $('#OpenProductTypeLookupTableButton').click(function () {
      var product = _$productInformationForm.serializeFormToObject();

      _ProductproductTypeLookupTableModal.open(
        { id: product.productTypeId, displayName: product.productTypeName },
        function (data) {
          _$productInformationForm.find('input[name=productTypeName]').val(data.displayName);
          _$productInformationForm.find('input[name=productTypeId]').val(data.id);
        },
      );
    });

    $('#ClearProductTypeNameButton').click(function () {
      _$productInformationForm.find('input[name=productTypeName]').val('');
      _$productInformationForm.find('input[name=productTypeId]').val('');
    });

    $('#OpenBrandLookupTableButton').click(function () {
      var product = _$productInformationForm.serializeFormToObject();

      _ProductbrandLookupTableModal.open({ id: product.brandId, displayName: product.brandName }, function (data) {
        _$productInformationForm.find('input[name=brandName]').val(data.displayName);
        _$productInformationForm.find('input[name=brandId]').val(data.id);
      });
    });

    $('#ClearBrandNameButton').click(function () {
      _$productInformationForm.find('input[name=brandName]').val('');
      _$productInformationForm.find('input[name=brandId]').val('');
    });

    this.save = function () {
      if (!_$productInformationForm.valid()) {
        return;
      }
      if ($('#Product_ProductTypeId').prop('required') && $('#Product_ProductTypeId').val() == '') {
        abp.message.error(app.localize('{0}IsRequired', app.localize('ProductType')));
        return;
      }
      if ($('#Product_BrandId').prop('required') && $('#Product_BrandId').val() == '') {
        abp.message.error(app.localize('{0}IsRequired', app.localize('Brand')));
        return;
      }

      if (_fileUploading != null && _fileUploading.length > 0) {
        abp.notify.info(app.localize('WaitingForFileUpload'));
        return;
      }

      var product = _$productInformationForm.serializeFormToObject();

      product.imageToken = _imageToken;

      _modalManager.setBusy(true);
      _productsService
        .createOrEdit(product)
        .done(function () {
          abp.notify.info(app.localize('SavedSuccessfully'));
          _modalManager.close();
          abp.event.trigger('app.createOrEditProductModalSaved');
        })
        .always(function () {
          _modalManager.setBusy(false);
        });
    };

    $('#Product_Image').change(function () {
      var file = $(this)[0].files[0];
      if (!file) {
        _imageToken = null;
        return;
      }

      var formData = new FormData();
      formData.append('file', file);
      _fileUploading.push(true);

      $.ajax({
        url: '/App/Products/UploadImageFile',
        type: 'POST',
        data: formData,
        processData: false,
        contentType: false,
      })
        .done(function (resp) {
          if (resp.success && resp.result.fileToken) {
            _imageToken = resp.result.fileToken;
          } else {
            abp.message.error(resp.result.message);
          }
        })
        .always(function () {
          _fileUploading.pop();
        });
    });

    $('#Product_Image_Remove').click(function () {
      abp.message.confirm(app.localize('DoYouWantToRemoveTheFile'), app.localize('AreYouSure'), function (isConfirmed) {
        if (isConfirmed) {
          var Product = _$productInformationForm.serializeFormToObject();
          _productsService
            .removeImageFile({
              id: Product.id,
            })
            .done(function () {
              abp.notify.success(app.localize('SuccessfullyDeleted'));
              _$productInformationForm.find('#div_current_file').css('display', 'none');
            });
        }
      });
    });

    $('#Product_Image').change(function () {
      var fileName = app.localize('ChooseAFile');
      if (this.files && this.files[0]) {
        fileName = this.files[0].name;
      }
      $('#Product_ImageLabel').text(fileName);
    });
  };
})(jQuery);
