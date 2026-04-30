import { faTrash } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { notifySaved } from 'components/alert/Toast';
import Table from 'domain/PayRuns/PayRun/table';
import queryString from 'query-string';
import React, { Fragment, useEffect, useState } from 'react';
import { Button, Card, CardBody, Col, Row } from 'reactstrap';
import TestEmployeeService from 'services/OnlineTest/Paper/TestEmployee';
import * as formUtil from 'utils/form';


export default function EmployeeList(props) {
    const Columns = [
        {
            Header: 'Employee No',
            accessor: 'employeeCode'
        },
        {
            Header: 'Employee Name',
            accessor: 'employee'
        },
        {
            Header: 'Designation',
            accessor: 'employeeDesig'
        },
        {
            Header: 'Department',
            accessor: 'employeeDept'
        },
        {
            Header: 'Delete',
            accessor: 'delete',
            Cell: ({ row }) => <FontAwesomeIcon icon={faTrash} style={{ color: 'red' }}
                onClick={() => handleDelete(row.original.id)} />
        }

    ]
    const [state, setState] = useState({
        isLoading: true, emps: [], pages: -1, title: '',
        pageSize: 10, hasNext: false, hasPrevious: false
    })

    const SearchData = async (page, pageSize, sortBy, isDescend) => {
        let pages = '', hasNext = false, hasPrevious = false, pageIndex = 0;

        const qString = queryString.stringify({
            page: page || 0,
            size: pageSize || state.pageSize,
            refId: props.refId,
            name: state.empNo,
            sortBy: sortBy || '', isDescend: isDescend || 'false'
        });
        await TestEmployeeService.Paginate(qString).then((res) => {
            pages = res.data.pages;
            hasNext = res.data.hasNext;
            hasPrevious = res.data.hasPrevious;
            pageIndex = res.data.index
            setState({
                ...state,
                isLoading: false, emps: res.data.items,
                pages: pages, hasNext: hasNext, hasPrevious: hasPrevious,
                pageIndex: pageIndex
            })
        })
    }

    useEffect(() => {
        SearchData();
    }, [])


    const handleDelete = async (testEmpId) => {
        await TestEmployeeService.Delete(testEmpId).then((res) => {
            notifySaved('Deleted Successfully');
            SearchData();
        }).catch((err) => {
            formUtil.displayErrors(err)
        })
    }

    return <Fragment>
        <Card className='mb-4'>
            <Row>
                <Col md='10'></Col>
                <Col md='2'>
                    <Button className="mb-2 mr-2 btn-icon float-right btn-xs btn-primary1" color="primary" type="button"
                        onClick={() => props.handleAddEmp()}
                    >
                        <i className="pe-7s-plus btn-icon-wrapper font-weight-bold" />
                        ADD
                    </Button>
                </Col>
            </Row>
            <Card>
                <CardBody>
                    <Table columns={Columns} data={state.emps} pages={state.pages}
                        hasPrevious={state.hasPrevious} hasNext={state.hasNext} pageIndex={state.pageIndex}
                        showPaginate={true} handleSearch={SearchData} />
                </CardBody>
            </Card>
        </Card>
    </Fragment>
}