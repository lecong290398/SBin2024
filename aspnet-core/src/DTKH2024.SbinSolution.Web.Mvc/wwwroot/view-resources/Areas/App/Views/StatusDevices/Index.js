﻿(function () {
  $(function () {
    var _$statusDevicesTable = $('#StatusDevicesTable');
    var _statusDevicesService = abp.services.app.statusDevices;

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
        getStatusDevices();
      })
      .on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        $selectedDate.startDate = null;
        getStatusDevices();
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
        getStatusDevices();
      })
      .on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        $selectedDate.endDate = null;
        getStatusDevices();
      });

    var _permissions = {
      create: abp.auth.hasPermission('Pages.Administration.StatusDevices.Create'),
      edit: abp.auth.hasPermission('Pages.Administration.StatusDevices.Edit'),
      delete: abp.auth.hasPermission('Pages.Administration.StatusDevices.Delete'),
    };

    var _createOrEditModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/StatusDevices/CreateOrEditModal',
      scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/StatusDevices/_CreateOrEditModal.js',
      modalClass: 'CreateOrEditStatusDeviceModal',
    });

    var _viewStatusDeviceModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/StatusDevices/ViewstatusDeviceModal',
      modalClass: 'ViewStatusDeviceModal',
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

    var dataTable = _$statusDevicesTable.DataTable({
      paging: true,
      serverSide: true,
      processing: true,
      listAction: {
        ajaxFunction: _statusDevicesService.getAll,
        inputFilter: function () {
          return {
            filter: $('#StatusDevicesTableFilter').val(),
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
                  _viewStatusDeviceModal.open({ id: data.record.statusDevice.id });
                },
              },
              {
                text: app.localize('Edit'),
                visible: function () {
                  return _permissions.edit;
                },
                action: function (data) {
                  _createOrEditModal.open({ id: data.record.statusDevice.id });
                },
              },
              {
                text: app.localize('Delete'),
                visible: function () {
                  return _permissions.delete;
                },
                action: function (data) {
                  deleteStatusDevice(data.record.statusDevice);
                },
              },
            ],
          },
        },
        {
          targets: 2,
          data: 'statusDevice.name',
          name: 'name',
        },
        {
          targets: 3,
          data: 'statusDevice.color',
          name: 'color',
          render: function (color) {
            if (color) {
              return `<div style="width: 30px;height: 30px;background: ${color};border-radius: 50%;"></div>`
            }
            else return ""
          }
        },
      ],
    });

    function getStatusDevices() {
      dataTable.ajax.reload();
    }

    function deleteStatusDevice(statusDevice) {
      abp.message.confirm('', app.localize('AreYouSure'), function (isConfirmed) {
        if (isConfirmed) {
          _statusDevicesService
            .delete({
              id: statusDevice.id,
            })
            .done(function () {
              getStatusDevices(true);
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

    $('#CreateNewStatusDeviceButton').click(function () {
      _createOrEditModal.open();
    });

    $('#ExportToExcelButton').click(function () {
      _statusDevicesService
        .getStatusDevicesToExcel({
          filter: $('#StatusDevicesTableFilter').val(),
          nameFilter: $('#NameFilterId').val(),
        })
        .done(function (result) {
          app.downloadTempFile(result);
        });
    });

    abp.event.on('app.createOrEditStatusDeviceModalSaved', function () {
      getStatusDevices();
    });

    $('#GetStatusDevicesButton').click(function (e) {
      e.preventDefault();
      getStatusDevices();
    });

    $(document).keypress(function (e) {
      if (e.which === 13) {
        getStatusDevices();
      }
    });

    $('.reload-on-change').change(function (e) {
      getStatusDevices();
    });

    $('.reload-on-keyup').keyup(function (e) {
      getStatusDevices();
    });

    $('#btn-reset-filters').click(function (e) {
      $('.reload-on-change,.reload-on-keyup,#MyEntsTableFilter').val('');
      getStatusDevices();
    });
  });
})();
