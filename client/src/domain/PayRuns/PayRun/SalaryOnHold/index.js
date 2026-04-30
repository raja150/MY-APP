import { faDownload } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { notifyError, notifySaved } from 'components/alert/Toast';
import queryString from 'query-string';
import React, { Fragment, useEffect, useState } from 'react';
import { Button, Col, Modal, ModalBody, ModalFooter, Row } from 'reactstrap';
import PayMonthService from 'services/PayRoll/PayMonth';
import * as NumberUtil from 'utils/number';
import Table from '../table';
import { EmployeeImageProps, EmployeeImageCell } from 'domain/Organization/ImageDetails/ConstImage';

function Index(props) {
    const [state, setFrmState] = useState({ module: '', entityName: '', empNo: '', isLoading: true, columns: [], data: [], pages: -1, title: '', pageSize: 10, hasNext: false, hasPrevious: false })
    const [tableLoading, setTableLoading] = useState(true)
    const [searchData, setSearchData] = useState({})
    const [sheetId, setSheetId] = useState('')
    const [modal, setModal] = useState(false);
    const toggle = () => setModal(!modal);

    useEffect(() => {
        const fetchData = async () => {
            let data = [], pages = '', hasNext = false, hasPrevious = false, pageIndex = 0;
            const qString = queryString.stringify({
                page: 0,
                size: 10,
                refId: props.rid
            });

            await PayMonthService.getHoldSalEmps(qString).then(res => {
                data = res.data.items;
                pages = res.data.pages;
                hasNext = res.data.hasNext;
                hasPrevious = res.data.hasPrevious;
                pageIndex = res.data.index

            }).catch(err => {
                notifyError(err.message)
            })

            setFrmState({ ...state, isLoading: false, data: data, pages: pages, hasNext: hasNext, hasPrevious: hasPrevious, pageIndex: pageIndex })
        }
        fetchData();
    }, []);

    const COLUMNS = [
        { Header: 'id', accessor: 'id', show: false },
        {
           ...EmployeeImageProps,
            Cell: ({ row }) => EmployeeImageCell(row.original.employeeID)
        },
        {
            Header: 'Employee', accessor: 'Employee.no', width: 250, Cell: cell => {
                const currentRow = cell.row.original
                return (
                    <div>
                        <div className='d-flex justify-content-between'>
                            <span>{currentRow.no}</span>
                            <span className='text-right'>{currentRow.name}</span>
                        </div>
                        <div className='d-flex justify-content-between' style={{ fontSize: '12px', color: '#999999' }}>
                            <span>{currentRow.department}</span>{" "}
                            <span className='text-right'>{currentRow.designation}</span>
                        </div>
                    </div>
                )
            }
        },
        { Header: 'Salary', accessor: 'salary', Cell: ({ value }) => NumberUtil.MoneyFormatWithDecimal(value) },
        { Header: 'Gross', accessor: 'gross', },
        { Header: 'Deduction', accessor: 'deduction', },
        { Header: 'Tax', accessor: 'tax', },
        { Header: 'Net', accessor: 'net', },
        { Header: 'Action', accessor: 'button', name: 'Release', Cell: ({ row }) => handleOnEdit(row.original) }
    ]
    const handleSearch = async (page, pageSize, sortBy, isDescend) => {
        const qString = queryString.stringify({
            ...searchData, page,
            size: pageSize || state.pageSize,
            name: state.empNo,
            refId: props.rid,
            sortBy: sortBy || '', isDescend: isDescend || 'false'
        });
        setTableLoading(true);

        await PayMonthService.getHoldSalEmps(qString).then((response) => {
            setFrmState({
                ...state, pages: response.data.pages, data: response.data.items, pageSize: pageSize || state.pageSize,
                hasNext: response.data.hasNext, hasPrevious: response.data.hasPrevious, pageIndex: response.data.index
            });
        });
        setTableLoading(false);
    }

    const handleOnChange = (name, value) => {
        setFrmState({ ...state, [name]: value, empNo: value })
    }
    const handleOnEdit = async (row) => {
        toggle();
        await setSheetId(row.id)
    }
    const handleRelease = async () => {
        await PayMonthService.ReleasePaySheet(sheetId).then(res => {
            notifySaved('Released Successfully')
        }).catch(e => {
            notifyError()
        })
        window.location.reload()
        toggle();
        await setSheetId('')
    }
    return (
        <Fragment>
            <Row>
                <Col xs='3'>
                    <div className="input-icon mr-3">
                        <input name="name" className="form-control form-control-solid bg-white" placeholder="Search Name" onChange={(e) => handleOnChange(e.target.name, e.target.value)} />
                        <span onClick={(e) => handleSearch(0)}>
                            <i className="pe-7s-search text-muted"></i>
                        </span>
                    </div>
                </Col>
                <Col xs='8'></Col>
                <Col xs='1'>
                    <FontAwesomeIcon icon={faDownload} size='2x' />
                </Col>
            </Row>
            <Table handleSearch={handleSearch} columns={COLUMNS} data={state.data} pageIndex={state.pageIndex} pages={state.pages} hasPrevious={state.hasPrevious} hasNext={state.hasNext} showPaginate={true} handleOnEdit={handleOnEdit} />

            <Modal isOpen={modal} toggle={toggle} >
                <ModalBody>
                </ModalBody>
                <h3 className='text-center'>Are you sure to release ?</h3>
                <ModalFooter>
                    <Button color="primary" onClick={() => handleRelease()}>Submit</Button>{' '}
                    <Button color="secondary" onClick={toggle}>Cancel</Button>
                </ModalFooter>
            </Modal>
        </Fragment>
    )
}

export default Index
