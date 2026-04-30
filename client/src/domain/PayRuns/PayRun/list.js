import { RWDropdownList } from 'components/dynamicform/Controls';
import Loading from 'components/Loader';
import moment from 'moment';
import queryString from 'query-string';
import React, { Fragment, useEffect, useMemo, useState } from 'react';
import { Card, CardBody, Col, Row } from 'reactstrap';
import APIService from 'services/apiservice';
import FinancialYear from '../../../services/PayRoll/FinancialYear'
import * as crypto from 'utils/Crypto';
import * as formUtil from 'utils/form';
import Table from './table';

export const Columns = [
    { Header: 'id', accessor: 'id', show: false },
    { Header: "Year", accessor: 'year' },
    { Header: 'Name', accessor: 'name' },
    { Header: 'Days', accessor: 'days' },
    { Header: 'Status', accessor: 'statusTxt' },
    // {
    //     Header: () => (
    //         <div className='text-right'>No of Employees</div>
    //     ),
    //     accessor: 'no',
    //     Cell: ({ value }) => (
    //         <div className='text-right'>{value}</div>
    //     )
    // },
    {
        Header: 'Action', accessor: 'button', name: 'Details', Cell: ({ value }) => (
            <div className='text-center'>{value}</div>
        )
    }
]

export const Data = [
    {
        id: 1,
        netPay: 78122.00,
        date: moment(new Date()).format('DD/MM/yyyy'),
        no: 1,
    }
]

function List(props) {
    const [state, setFrmState] = useState({ financialYear: '', payMonths: [], module: '', entityName: '', isLoading: true, columns: [], data: [], pages: -1, title: '', pageSize: 10, hasNext: false, hasPrevious: false, financialYears: [] })
    const columns = useMemo(() => Columns, []);
    const data = useMemo(() => Data, []);

    useEffect(() => {
        const fetchData = async () => {
            let payMonths = [], pages = '', hasNext = false, hasPrevious = false, pageIndex = 0, financialYears = [];
            const qString = queryString.stringify({
                page: 0,
                size: 10,
            });
            await FinancialYear.getList().then((result) => {
                financialYears = result.data
            })
            await APIService.getAsync(`PayRoll/PayMonth/PayMonthsList?${qString}`).then((res) => {
                payMonths = res.data.items
                pages = res.data.pages;
                hasNext = res.data.hasNext;
                hasPrevious = res.data.hasPrevious;
                pageIndex = res.data.index

            }).catch(err => {
                formUtil.displayErrors(err);
            })
            setFrmState({ ...state, isLoading: false, payMonths: payMonths, pages: pages, hasNext: hasNext, hasPrevious: hasPrevious, pageIndex: pageIndex, financialYears: financialYears })
        }
        fetchData();
    }, []);

    const handleOnEdit = (n) => {
        const qry = { r: (n.id ? crypto.encrypt(n.id) : '') };
        props.history.push('/m/Payroll/Payrun/details?' + queryString.stringify(qry));
    }
    const handleSearch = async (page, pageSize) => {
        let payMonths = [], pages = '', hasNext = false, hasPrevious = false, pageIndex = 0, financialYears = [];
        const qString = queryString.stringify({
            page: page,
            size: pageSize || state.pageSize, //10,
        });
        await FinancialYear.getList().then((result) => {
            financialYears = result.data
        })
        await APIService.getAsync(`PayRoll/PayMonth/PayMonthsList?${qString}`).then((res) => {
            payMonths = res.data.items
            pages = res.data.pages;
            hasNext = res.data.hasNext;
            hasPrevious = res.data.hasPrevious;
            pageIndex = res.data.index

        })
        setFrmState({ ...state, isLoading: false, payMonths: payMonths, pages: pages, hasNext: hasNext, hasPrevious: hasPrevious, pageIndex: pageIndex, financialYears: financialYears })

    }
    const handleValueChange = async (name, value, { selected }) => {
        setFrmState({ ...state, isLoading: false });
        let payMonths = [], pages = '', hasNext = false, hasPrevious = false, pageIndex = 0;
        if (selected !== undefined) {
            const qString = queryString.stringify({
                page: 0,
                size: 10,
                refId: selected.id
            });
            await APIService.getAsync(`PayRoll/PayMonth/PayMonthsList?${qString}`).then(res => {
                payMonths = res.data.items;
            }).catch(err => {
                formUtil.displayErrors(err);
            })
        } else {
            await APIService.getAsync('PayRoll/PayMonth/PayMonthsList').then((res) => {
                payMonths = res.data.items;
                pages = res.data.pages;
                hasNext = res.data.hasNext;
                hasPrevious = res.data.hasPrevious;
                pageIndex = res.data.index
            }).catch(err => {
                formUtil.displayErrors(err);
            })
        }

        setFrmState({ ...state, isLoading: false, payMonths: payMonths, [name]: value, pages: pages, hasNext: hasNext, hasPrevious: hasPrevious, pageIndex: pageIndex, })
    }
    return (
        <Fragment>
            {
                state.isLoading ? <Loading /> :
                    <Fragment>
                        <div className="app-page-title pt-2 pb-2">
                            <div className="page-title-wrapper">
                                <div className="page-title-heading">
                                    <div>Pay run</div>
                                </div>

                            </div>
                        </div>
                        <Card>
                            <CardBody>
                                <Row>
                                    <Col xs='6'>
                                        <RWDropdownList {...{
                                            name: 'financialYear', label: 'Financial Year', valueField: 'id', textField: 'name',
                                            value: state.financialYear, type: 'string', values: state.financialYears,

                                        }} handlevaluechange={handleValueChange} />
                                    </Col>
                                </Row>
                                <Table columns={columns} data={state.payMonths} pages={state.pages}
                                    hasPrevious={state.hasPrevious} hasNext={state.hasNext} pageIndex={state.pageIndex}
                                    showPaginate={true} handleOnEdit={handleOnEdit} handleSearch={handleSearch} />
                            </CardBody>
                        </Card>
                    </Fragment>
            }
        </Fragment>
    )
}

export default List
