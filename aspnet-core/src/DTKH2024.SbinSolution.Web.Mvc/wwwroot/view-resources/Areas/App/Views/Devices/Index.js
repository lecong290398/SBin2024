﻿(function () {
    $(function () {
        var _$devicesTable = $('#DevicesTable');
        var _devicesService = abp.services.app.devices;

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
                getDevices();
            })
            .on('cancel.daterangepicker', function (ev, picker) {
                $(this).val('');
                $selectedDate.startDate = null;
                getDevices();
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
                getDevices();
            })
            .on('cancel.daterangepicker', function (ev, picker) {
                $(this).val('');
                $selectedDate.endDate = null;
                getDevices();
            });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Administration.Devices.Create'),
            edit: abp.auth.hasPermission('Pages.Administration.Devices.Edit'),
            delete: abp.auth.hasPermission('Pages.Administration.Devices.Delete'),
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/Devices/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/Devices/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditDeviceModal',
        });

        var _viewDeviceModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/Devices/ViewdeviceModal',
            modalClass: 'ViewDeviceModal',
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

        var dataTable = _$devicesTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _devicesService.getAll,
                inputFilter: function () {
                    return {
                        filter: $('#DevicesTableFilter').val(),
                        nameFilter: $('#NameFilterId').val(),
                        sensorPlastisAvailableFilter: $('#SensorPlastisAvailableFilterId').val(),
                        sensorMetalAvailableFilter: $('#SensorMetalAvailableFilterId').val(),
                        statusDeviceNameFilter: $('#StatusDeviceNameFilterId').val(),
                        userNameFilter: $('#UserNameFilterId').val(),
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
                                    _viewDeviceModal.open({ id: data.record.device.id });
                                },
                            },
                            {
                                text: app.localize('Edit'),
                                visible: function () {
                                    return _permissions.edit;
                                },
                                action: function (data) {
                                    _createOrEditModal.open({ id: data.record.device.id });
                                },
                            },
                            {
                                text: app.localize('Delete'),
                                visible: function () {
                                    return _permissions.delete;
                                },
                                action: function (data) {
                                    deleteDevice(data.record.device);
                                },
                            },
                        ],
                    },
                },
                {
                    targets: 2,
                    data: 'device.name',
                    name: 'name',
                },
                {
                    targets: 3,
                    data: 'device.plastisPoint',
                    name: 'plastisPoint',
                },
                {
                    targets: 4,
                    data: 'device.sensorPlastisAvailable',
                    name: 'sensorPlastisAvailable',
                    render: function (sensorPlastisAvailable) {
                        if (sensorPlastisAvailable) {
                            return '<div class="text-center"><i class="fa fa-check text-success" title="True"></i></div>';
                        }
                        return '<div class="text-center"><i class="fa fa-times-circle" title="False"></i></div>';
                    },
                },
                {
                    targets: 5,
                    data: 'device.percentStatusPlastis',
                    name: 'percentStatusPlastis',
                    render: function (percentStatusPlastis) {
                        if (percentStatusPlastis == 1) {
                            return '<div class="text-center">' + (app.localize('FullTrash')) + '</div>';
                        }
                        return '<div class="text-center">' + (app.localize('FullNotTrash')) + '</div>';
                    }
                },
                {
                    targets: 6,
                    data: 'device.metalPoint',
                    name: 'metalPoint',
                },
                {
                    targets: 7,
                    data: 'device.sensorMetalAvailable',
                    name: 'sensorMetalAvailable',
                    render: function (sensorMetalAvailable) {
                        if (sensorMetalAvailable) {
                            return '<div class="text-center"><i class="fa fa-check text-success" title="True"></i></div>';
                        }
                        return '<div class="text-center"><i class="fa fa-times-circle" title="False"></i></div>';
                    },
                },
                {
                    targets: 8,
                    data: 'device.percentStatusMetal',
                    name: 'percentStatusMetal',
                    render: function (percentStatusMetal) {
                        if (percentStatusMetal == 1) {
                            return '<div class="text-center">' + (app.localize('FullTrash')) + '</div>';
                        }
                        return '<div class="text-center">' + (app.localize('FullNotTrash')) + '</div>';
                    }
                },
                {
                    targets: 9,
                    data: 'device.percentStatusOrther',
                    name: 'percentStatusOrther',
                    render: function (percentStatusOrther) {
                        if (percentStatusOrther == 1) {
                            return '<div class="text-center">' + (app.localize('FullTrash')) + '</div>';
                        }
                        return '<div class="text-center">' + (app.localize('FullNotTrash')) + '</div>';
                    }
                },
                {
                    targets: 10,
                    data: 'device.errorPoint',
                    name: 'errorPoint',
                },
                {
                    targets: 11,
                    data: 'device.address',
                    name: 'address',
                },
                {
                    targets: 12,
                    data: 'statusDeviceName',
                    name: 'statusDeviceFk.name',
                },
                {
                    targets: 13,
                    data: 'userName',
                    name: 'userFk.name',
                },
                {
                    targets: 14,
                    data: 'device.lastModificationTime',
                    name: 'lastModificationTime',
                },
            ],
        });

        function getDevices() {
            dataTable.ajax.reload();
        }

        function deleteDevice(device) {
            abp.message.confirm('', app.localize('AreYouSure'), function (isConfirmed) {
                if (isConfirmed) {
                    _devicesService
                        .delete({
                            id: device.id,
                        })
                        .done(function () {
                            getDevices(true);
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

        $('#CreateNewDeviceButton').click(function () {
            _createOrEditModal.open();
        });

        $('#ExportToExcelButton').click(function () {
            _devicesService
                .getDevicesToExcel({
                    filter: $('#DevicesTableFilter').val(),
                    nameFilter: $('#NameFilterId').val(),
                    sensorPlastisAvailableFilter: $('#SensorPlastisAvailableFilterId').val(),
                    sensorMetalAvailableFilter: $('#SensorMetalAvailableFilterId').val(),
                    statusDeviceNameFilter: $('#StatusDeviceNameFilterId').val(),
                    userNameFilter: $('#UserNameFilterId').val(),
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditDeviceModalSaved', function () {
            getDevices();
        });

        $('#GetDevicesButton').click(function (e) {
            e.preventDefault();
            getDevices();
        });

        $(document).keypress(function (e) {
            if (e.which === 13) {
                getDevices();
            }
        });

        $('.reload-on-change').change(function (e) {
            getDevices();
        });

        $('.reload-on-keyup').keyup(function (e) {
            getDevices();
        });

        $('#btn-reset-filters').click(function (e) {
            $('.reload-on-change,.reload-on-keyup,#MyEntsTableFilter').val('');
            getDevices();
        });
    });
})();
