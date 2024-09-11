(function () {
  $(function () {
    var _$productsTable = $('#ProductsTable');
    var _productsService = abp.services.app.products;

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
        getProducts();
      })
      .on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        $selectedDate.startDate = null;
        getProducts();
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
        getProducts();
      })
      .on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        $selectedDate.endDate = null;
        getProducts();
      });

    var _permissions = {
      create: abp.auth.hasPermission('Pages.Administration.Products.Create'),
      edit: abp.auth.hasPermission('Pages.Administration.Products.Edit'),
      delete: abp.auth.hasPermission('Pages.Administration.Products.Delete'),
    };

    var _createOrEditModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/Products/CreateOrEditModal',
      scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/Products/_CreateOrEditModal.js',
      modalClass: 'CreateOrEditProductModal',
    });

    var _viewProductModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/Products/ViewproductModal',
      modalClass: 'ViewProductModal',
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

    var dataTable = _$productsTable.DataTable({
      paging: true,
      serverSide: true,
      processing: true,
      listAction: {
        ajaxFunction: _productsService.getAll,
        inputFilter: function () {
          return {
            filter: $('#ProductsTableFilter').val(),
            productNameFilter: $('#ProductNameFilterId').val(),
            productTypeNameFilter: $('#ProductTypeNameFilterId').val(),
            brandNameFilter: $('#BrandNameFilterId').val(),
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
                  _viewProductModal.open({ id: data.record.product.id });
                },
              },
              {
                text: app.localize('Edit'),
                visible: function () {
                  return _permissions.edit;
                },
                action: function (data) {
                  _createOrEditModal.open({ id: data.record.product.id });
                },
              },
              {
                text: app.localize('Delete'),
                visible: function () {
                  return _permissions.delete;
                },
                action: function (data) {
                  deleteProduct(data.record.product);
                },
              },
            ],
          },
        },
        {
          targets: 2,
          data: 'product.productName',
          name: 'productName',
        },
        {
          targets: 3,
          data: 'product.timeDescription',
          name: 'timeDescription',
        },
        {
          targets: 4,
          data: 'product.applicableSubjects',
          name: 'applicableSubjects',
        },
        {
          targets: 5,
          data: 'product.regulations',
          name: 'regulations',
        },
        {
          targets: 6,
          data: 'product.userManual',
          name: 'userManual',
        },
        {
          targets: 7,
          data: 'product.scopeOfApplication',
          name: 'scopeOfApplication',
        },
        {
          targets: 8,
          data: 'product.supportAndComplaints',
          name: 'supportAndComplaints',
        },
        {
          targets: 9,
          data: 'product.description',
          name: 'description',
        },
        {
          targets: 10,
          data: 'product',
          render: function (product) {
            if (!product.image) {
              return '';
            }
            return `<a href="/File/DownloadBinaryFile?id=${product.image}" target="_blank">${product.imageFileName}</a>`;
          },
        },
        {
          targets: 11,
          data: 'productTypeName',
          name: 'productTypeFk.name',
        },
        {
          targets: 12,
          data: 'brandName',
          name: 'brandFk.name',
        },
      ],
    });

    function getProducts() {
      dataTable.ajax.reload();
    }

    function deleteProduct(product) {
      abp.message.confirm('', app.localize('AreYouSure'), function (isConfirmed) {
        if (isConfirmed) {
          _productsService
            .delete({
              id: product.id,
            })
            .done(function () {
              getProducts(true);
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

    $('#CreateNewProductButton').click(function () {
      _createOrEditModal.open();
    });

    $('#ExportToExcelButton').click(function () {
      _productsService
        .getProductsToExcel({
          filter: $('#ProductsTableFilter').val(),
          productNameFilter: $('#ProductNameFilterId').val(),
          productTypeNameFilter: $('#ProductTypeNameFilterId').val(),
          brandNameFilter: $('#BrandNameFilterId').val(),
        })
        .done(function (result) {
          app.downloadTempFile(result);
        });
    });

    abp.event.on('app.createOrEditProductModalSaved', function () {
      getProducts();
    });

    $('#GetProductsButton').click(function (e) {
      e.preventDefault();
      getProducts();
    });

    $(document).keypress(function (e) {
      if (e.which === 13) {
        getProducts();
      }
    });

    $('.reload-on-change').change(function (e) {
      getProducts();
    });

    $('.reload-on-keyup').keyup(function (e) {
      getProducts();
    });

    $('#btn-reset-filters').click(function (e) {
      $('.reload-on-change,.reload-on-keyup,#MyEntsTableFilter').val('');
      getProducts();
    });
  });
})();
