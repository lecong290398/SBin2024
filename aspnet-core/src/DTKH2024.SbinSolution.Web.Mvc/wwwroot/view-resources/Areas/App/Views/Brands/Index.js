﻿(function () {
  $(function () {
    var _$brandsTable = $('#BrandsTable');
    var _brandsService = abp.services.app.brands;

    var $selectedDate = {
      startDate: null,
      endDate: null,
    };

    $('.date-picker').on('apply.daterangepicker', function (ev, picker) {
      $(this).val(picker.startDate.format('MM/DD/YYYY'));
    });

    $('.startDate')
      .daterangepicker({
        autoUpdateInput: false,
        singleDatePicker: true,
        locale: abp.localization.currentLanguage.name,
        format: 'L',
      })
      .on('apply.daterangepicker', (ev, picker) => {
        $selectedDate.startDate = picker.startDate;
        getBrands();
      })
      .on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        $selectedDate.startDate = null;
        getBrands();
      });

    $('.endDate')
      .daterangepicker({
        autoUpdateInput: false,
        singleDatePicker: true,
        locale: abp.localization.currentLanguage.name,
        format: 'L',
      })
      .on('apply.daterangepicker', (ev, picker) => {
        $selectedDate.endDate = picker.startDate;
        getBrands();
      })
      .on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        $selectedDate.endDate = null;
        getBrands();
      });

    var _permissions = {
      create: abp.auth.hasPermission('Pages.Administration.Brands.Create'),
      edit: abp.auth.hasPermission('Pages.Administration.Brands.Edit'),
      delete: abp.auth.hasPermission('Pages.Administration.Brands.Delete'),
    };

    var _createOrEditModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/Brands/CreateOrEditModal',
      scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/Brands/_CreateOrEditModal.js',
      modalClass: 'CreateOrEditBrandModal',
    });

    var _viewBrandModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/Brands/ViewbrandModal',
      modalClass: 'ViewBrandModal',
    });

    var getDateFilter = function (element) {
      if ($selectedDate.startDate == null) {
        return null;
      }
      return $selectedDate.startDate.format('YYYY-MM-DDT00:00:00Z');
    };

    var getMaxDateFilter = function (element) {
      if ($selectedDate.endDate == null) {
        return null;
      }
      return $selectedDate.endDate.format('YYYY-MM-DDT23:59:59Z');
    };

    var dataTable = _$brandsTable.DataTable({
      paging: true,
      serverSide: true,
      processing: true,
      listAction: {
        ajaxFunction: _brandsService.getAll,
        inputFilter: function () {
          return {
            filter: $('#BrandsTableFilter').val(),
            nameFilter: $('#NameFilterId').val(),
          };
        },
      },
      columnDefs: [
        {
          className: 'control responsive',
          orderable: false,
          render: function () {
            return '';
          },
          targets: 0,
        },
        {
          width: 120,
          targets: 1,
          data: null,
          orderable: false,
          autoWidth: false,
          defaultContent: '',
          rowAction: {
            cssClass: 'btn btn-brand dropdown-toggle',
            text: '<i class="fa fa-cog"></i> ' + app.localize('Actions') + ' <span class="caret"></span>',
            items: [
              {
                text: app.localize('View'),
                action: function (data) {
                  _viewBrandModal.open({ id: data.record.brand.id });
                },
              },
              {
                text: app.localize('Edit'),
                visible: function () {
                  return _permissions.edit;
                },
                action: function (data) {
                  _createOrEditModal.open({ id: data.record.brand.id });
                },
              },
              {
                text: app.localize('Delete'),
                visible: function () {
                  return _permissions.delete;
                },
                action: function (data) {
                  deleteBrand(data.record.brand);
                },
              },
            ],
          },
        },
        {
          targets: 2,
          data: 'brand.name',
          name: 'name',
        },
        {
          targets: 3,
          data: 'brand.description',
          name: 'description',
        },
        {
          targets: 4,
          data: 'brand',
          render: function (brand) {
            if (!brand.logo) {
              return '';
            }
            return `<a href="/File/DownloadBinaryFile?id=${brand.logo}" target="_blank">${brand.logoFileName}</a>`;
          },
        },
      ],
    });

    function getBrands() {
      dataTable.ajax.reload();
    }

    function deleteBrand(brand) {
      abp.message.confirm('', app.localize('AreYouSure'), function (isConfirmed) {
        if (isConfirmed) {
          _brandsService
            .delete({
              id: brand.id,
            })
            .done(function () {
              getBrands(true);
              abp.notify.success(app.localize('SuccessfullyDeleted'));
            });
        }
      });
    }

    $('#ShowAdvancedFiltersSpan').click(function () {
      $('#ShowAdvancedFiltersSpan').hide();
      $('#HideAdvancedFiltersSpan').show();
      $('#AdvacedAuditFiltersArea').slideDown();
    });

    $('#HideAdvancedFiltersSpan').click(function () {
      $('#HideAdvancedFiltersSpan').hide();
      $('#ShowAdvancedFiltersSpan').show();
      $('#AdvacedAuditFiltersArea').slideUp();
    });

    $('#CreateNewBrandButton').click(function () {
      _createOrEditModal.open();
    });

    $('#ExportToExcelButton').click(function () {
      _brandsService
        .getBrandsToExcel({
          filter: $('#BrandsTableFilter').val(),
          nameFilter: $('#NameFilterId').val(),
        })
        .done(function (result) {
          app.downloadTempFile(result);
        });
    });

    abp.event.on('app.createOrEditBrandModalSaved', function () {
      getBrands();
    });

    $('#GetBrandsButton').click(function (e) {
      e.preventDefault();
      getBrands();
    });

    $(document).keypress(function (e) {
      if (e.which === 13) {
        getBrands();
      }
    });

    $('.reload-on-change').change(function (e) {
      getBrands();
    });

    $('.reload-on-keyup').keyup(function (e) {
      getBrands();
    });

    $('#btn-reset-filters').click(function (e) {
      $('.reload-on-change,.reload-on-keyup,#MyEntsTableFilter').val('');
      getBrands();
    });
  });
})();
