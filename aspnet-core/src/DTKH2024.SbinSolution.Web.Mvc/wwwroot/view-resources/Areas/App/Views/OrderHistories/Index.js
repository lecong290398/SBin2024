﻿(function () {
    $(function () {
        var _$orderHistoriesTable = $('#OrderHistoriesTable');
        var _orderHistoriesService = abp.services.app.orderHistories;

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
                getOrderHistories();
            })
            .on('cancel.daterangepicker', function (ev, picker) {
                $(this).val('');
                $selectedDate.startDate = null;
                getOrderHistories();
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
                getOrderHistories();
            })
            .on('cancel.daterangepicker', function (ev, picker) {
                $(this).val('');
                $selectedDate.endDate = null;
                getOrderHistories();
            });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.OrderHistories.Create'),
            edit: abp.auth.hasPermission('Pages.OrderHistories.Edit'),
            delete: abp.auth.hasPermission('Pages.OrderHistories.Delete'),
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/OrderHistories/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/OrderHistories/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditOrderHistoryModal',
        });

        var _viewOrderHistoryModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/OrderHistories/VieworderHistoryModal',
            modalClass: 'ViewOrderHistoryModal',
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

        var dataTable = _$orderHistoriesTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _orderHistoriesService.getAll,
                inputFilter: function () {
                    return {
                        filter: $('#OrderHistoriesTableFilter').val(),
                        userNameFilter: $('#UserNameFilterId').val(),
                        transactionBinTransactionCodeFilter: $('#TransactionBinTransactionCodeFilterId').val(),
                        wareHouseGiftCodeFilter: $('#WareHouseGiftCodeFilterId').val(),
                        historyTypeNameFilter: $('#HistoryTypeNameFilterId').val(),
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
                                    _viewOrderHistoryModal.open({ id: data.record.orderHistory.id });
                                },
                            },
                            {
                                text: app.localize('Edit'),
                                visible: function () {
                                    return _permissions.edit;
                                },
                                action: function (data) {
                                    _createOrEditModal.open({ id: data.record.orderHistory.id });
                                },
                            },
                            {
                                text: app.localize('Delete'),
                                visible: function () {
                                    return _permissions.delete;
                                },
                                action: function (data) {
                                    deleteOrderHistory(data.record.orderHistory);
                                },
                            },
                        ],
                    },
                },
                {
                    targets: 2,
                    data: 'orderHistory.description',
                    name: 'description',
                },
                {
                    targets: 3,
                    data: 'orderHistory.reason',
                    name: 'reason',
                },
                {
                    targets: 4,
                    data: 'orderHistory.point',
                    name: 'point',
                },
                {
                    targets: 5,
                    data: 'userName',
                    name: 'userFk.name',
                },
                {
                    targets: 6,
                    data: 'transactionBinTransactionCode',
                    name: 'transactionBinFk.transactionCode',
                },
                {
                    targets: 7,
                    data: 'wareHouseGiftCode',
                    name: 'wareHouseGiftFk.code',
                },
                {
                    targets: 8,
                    data: 'historyTypeName',
                    name: 'historyTypeFk.name',
                }, {
                    targets: 9,
                    data: 'orderHistory.creationTime',
                    name: 'creationTime',
                },
            ],
        });

        function getOrderHistories() {
            dataTable.ajax.reload();
        }

        function deleteOrderHistory(orderHistory) {
            abp.message.confirm('', app.localize('AreYouSure'), function (isConfirmed) {
                if (isConfirmed) {
                    _orderHistoriesService
                        .delete({
                            id: orderHistory.id,
                        })
                        .done(function () {
                            getOrderHistories(true);
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

        $('#CreateNewOrderHistoryButton').click(function () {
            _createOrEditModal.open();
        });

        $('#ExportToExcelButton').click(function () {
            _orderHistoriesService
                .getOrderHistoriesToExcel({
                    filter: $('#OrderHistoriesTableFilter').val(),
                    userNameFilter: $('#UserNameFilterId').val(),
                    transactionBinTransactionCodeFilter: $('#TransactionBinTransactionCodeFilterId').val(),
                    wareHouseGiftCodeFilter: $('#WareHouseGiftCodeFilterId').val(),
                    historyTypeNameFilter: $('#HistoryTypeNameFilterId').val(),
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditOrderHistoryModalSaved', function () {
            getOrderHistories();
        });

        $('#GetOrderHistoriesButton').click(function (e) {
            e.preventDefault();
            getOrderHistories();
        });

        $(document).keypress(function (e) {
            if (e.which === 13) {
                getOrderHistories();
            }
        });

        $('.reload-on-change').change(function (e) {
            getOrderHistories();
        });

        $('.reload-on-keyup').keyup(function (e) {
            getOrderHistories();
        });

        $('#btn-reset-filters').click(function (e) {
            $('.reload-on-change,.reload-on-keyup,#MyEntsTableFilter').val('');
            getOrderHistories();
        });
    });
})();
