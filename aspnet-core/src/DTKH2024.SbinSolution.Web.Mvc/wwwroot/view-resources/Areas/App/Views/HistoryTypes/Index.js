(function () {
  $(function () {
    var _$historyTypesTable = $('#HistoryTypesTable');
    var _historyTypesService = abp.services.app.historyTypes;

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
        getHistoryTypes();
      })
      .on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        $selectedDate.startDate = null;
        getHistoryTypes();
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
        getHistoryTypes();
      })
      .on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        $selectedDate.endDate = null;
        getHistoryTypes();
      });

    var _permissions = {
      create: abp.auth.hasPermission('Pages.Administration.HistoryTypes.Create'),
      edit: abp.auth.hasPermission('Pages.Administration.HistoryTypes.Edit'),
      delete: abp.auth.hasPermission('Pages.Administration.HistoryTypes.Delete'),
    };

    var _createOrEditModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/HistoryTypes/CreateOrEditModal',
      scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/HistoryTypes/_CreateOrEditModal.js',
      modalClass: 'CreateOrEditHistoryTypeModal',
    });

    var _viewHistoryTypeModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/HistoryTypes/ViewhistoryTypeModal',
      modalClass: 'ViewHistoryTypeModal',
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

    var dataTable = _$historyTypesTable.DataTable({
      paging: true,
      serverSide: true,
      processing: true,
      listAction: {
        ajaxFunction: _historyTypesService.getAll,
        inputFilter: function () {
          return {
            filter: $('#HistoryTypesTableFilter').val(),
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
                  _viewHistoryTypeModal.open({ id: data.record.historyType.id });
                },
              },
              {
                text: app.localize('Edit'),
                visible: function () {
                  return _permissions.edit;
                },
                action: function (data) {
                  _createOrEditModal.open({ id: data.record.historyType.id });
                },
              },
              {
                text: app.localize('Delete'),
                visible: function () {
                  return _permissions.delete;
                },
                action: function (data) {
                  deleteHistoryType(data.record.historyType);
                },
              },
            ],
          },
        },
        {
          targets: 2,
          data: 'historyType.name',
          name: 'name',
        },
        {
          targets: 3,
          data: 'historyType.description',
          name: 'description',
        },
        {
          targets: 4,
          data: 'historyType.color',
          name: 'color',
        },
      ],
    });

    function getHistoryTypes() {
      dataTable.ajax.reload();
    }

    function deleteHistoryType(historyType) {
      abp.message.confirm('', app.localize('AreYouSure'), function (isConfirmed) {
        if (isConfirmed) {
          _historyTypesService
            .delete({
              id: historyType.id,
            })
            .done(function () {
              getHistoryTypes(true);
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

    $('#CreateNewHistoryTypeButton').click(function () {
      _createOrEditModal.open();
    });

    $('#ExportToExcelButton').click(function () {
      _historyTypesService
        .getHistoryTypesToExcel({
          filter: $('#HistoryTypesTableFilter').val(),
          nameFilter: $('#NameFilterId').val(),
        })
        .done(function (result) {
          app.downloadTempFile(result);
        });
    });

    abp.event.on('app.createOrEditHistoryTypeModalSaved', function () {
      getHistoryTypes();
    });

    $('#GetHistoryTypesButton').click(function (e) {
      e.preventDefault();
      getHistoryTypes();
    });

    $(document).keypress(function (e) {
      if (e.which === 13) {
        getHistoryTypes();
      }
    });

    $('.reload-on-change').change(function (e) {
      getHistoryTypes();
    });

    $('.reload-on-keyup').keyup(function (e) {
      getHistoryTypes();
    });

    $('#btn-reset-filters').click(function (e) {
      $('.reload-on-change,.reload-on-keyup,#MyEntsTableFilter').val('');
      getHistoryTypes();
    });
  });
})();
