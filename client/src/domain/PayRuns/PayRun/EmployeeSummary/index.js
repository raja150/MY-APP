import { faDownload } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { notifyError, notifySaved } from 'components/alert/Toast';
import { Input, RWDropdownList } from 'components/dynamicform/Controls';
import MoneyFormat from 'components/Formats/MoneyFormat';
import Loading from 'components/Loader';
import SalaryDetails from 'domain/SelfService/Payslips/SalaryDetails';
import { FileDownload } from 'domain/Setup/DataImport/file-download';
import queryString from 'query-string';
import React, { Fragment, useEffect, useState } from 'react';
import { Button, Col, Dropdown, DropdownItem, DropdownMenu, DropdownToggle, Label, Modal, ModalBody, ModalFooter, Row } from 'reactstrap';
import PayMonthService from 'services/PayRoll/PayMonth';
import * as formUtil from 'utils/form';
import Table from '../table';
import { EmployeeImageCell, EmployeeImageProps } from 'domain/Organization/ImageDetails/ConstImage';
import Annexure from 'domain/SelfService/Payslips/Annexure';
import IncentivesPayCutService from 'services/PayRoll/IncentivesPayCut'

function Index(props) {

    const columns = [
        { Header: 'id', accessor: 'id', show: false },
        {
            ...EmployeeImageProps,
            Cell: ({ row }) => EmployeeImageCell(row.original.employeeID)
        },
        {
            Header: 'Employee', accessor: 'employee.no', width: 250, Cell: cell => {
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
        {
            Header: 'Salary', accessor: 'salary', Cell: cell => {
                const currentRow = cell.row.original
                return <div className='align-items-right'><MoneyFormat value={currentRow.salary} /></div>
            }
        },
        { Header: 'Gross', accessor: 'gross', },
        { Header: 'Deduction', accessor: 'deduction', },
        { Header: 'Tax', accessor: 'tax', },
        { Header: 'Net', accessor: 'net', },
        {
            Header: 'Action', accessor: 'payslip', Cell: cell => (
                <Dropdown isOpen={accordion[cell.row.index]} toggle={() => toggleAccordion(cell.row.index)}>
                    <DropdownToggle className="btn-icon btn-icon-only" color="link">
                        <i className="pe-7s-more btn-icon-wrapper" />
                    </DropdownToggle>
                    <DropdownMenu>
                        <DropdownItem onClick={() => handleOnEdit(cell.row.original)}>Hold</DropdownItem>
                        <DropdownItem onClick={() => handlePayroll(cell.row.original)}>Payslip</DropdownItem>
                        <DropdownItem onClick={() => downloadPaySlip(cell.row.original)}>PDF</DropdownItem>
                        <DropdownItem onClick={() => handleAnnexure(cell.row.original)}>Annexure</DropdownItem>
                    </DropdownMenu>
                </Dropdown>
            )
        }
    ]

    const [state, setFrmState] = useState({ module: '', entityName: '', empNo: '', isLoading: true, columns: [], data: [], pages: -1, title: '', pageSize: 10, hasNext: false, hasPrevious: false, filters: '' })
    const [tableLoading, setTableLoading] = useState(true)
    const [searchData, setSearchData] = useState({})
    const [incentivesAndPayCut, setIncentivesAndPayCut] = useState({});
    const [pwd, setPwd] = useState('')
    const [payMonthId, setPayMonthId] = useState('')
    const [employeeID, setEmployeeID] = useState('');
    const [dModal, setDModal] = useState(false)
    const [loading, setLoading] = useState(false)
    const [modal, setModal] = useState(false);
    const [paySlip, setPaySlip] = useState({})
    const toggle = () => setModal(!modal);

    const [payslipModal, setPayslipModal] = useState(false);
    const togglePayslip = () => setPayslipModal(!payslipModal)

    const [annexureModal, setAnnexureModal] = useState(false);
    const toggleAnnexure = () => setAnnexureModal(!annexureModal)


    const [accordion, setAccordion] = useState([]);
    const toggleAccordion = (tab) => {
        const state = accordion.map((x, index) => tab === index ? !x : false);
        setAccordion(state);
    }

    useEffect(() => {
        const fetchData = async () => {
            let data = [], pages = '', hasNext = false, hasPrevious = false, pageIndex = 0;

            const qString = queryString.stringify({
                page: 0,
                size: 10,
                refId: props.rid
            });

            await PayMonthService.getPayMonthEmpS(qString).then(res => {
                data = res.data.items;
                pages = res.data.pages;
                hasNext = res.data.hasNext;
                hasPrevious = res.data.hasPrevious;
                pageIndex = res.data.index;

                data.length > 0 && data.map(() => {
                    accordion.push(false)
                })

            }).catch(err => {
                formUtil.displayErrors(err);
            })

            setFrmState({ ...state, isLoading: false, data: data, pages: pages, hasNext: hasNext, hasPrevious: hasPrevious, pageIndex: pageIndex })
        }
        fetchData();
    }, []);

    const handleSearch = async (page, pageSize, sortBy, isDescend) => {
        const qString = queryString.stringify({
            ...searchData, page,
            size: pageSize || state.pageSize,
            refId: props.rid,
            name: state.empNo,
            sortBy: sortBy || '', isDescend: isDescend || 'false'
        });
        setTableLoading(true);
        await PayMonthService.getPayMonthEmpS(qString).then((response) => {
            const accordionTemp = [];
            response.data.items.length > 0 && response.data.items.map(() => {
                accordionTemp.push(false);
            })
            setAccordion(accordionTemp);
            setFrmState({
                ...state, pages: response.data.pages, data: response.data.items, pageSize: pageSize || state.pageSize,
                hasNext: response.data.hasNext, hasPrevious: response.data.hasPrevious, pageIndex: response.data.index
            });
        });
        setTableLoading(false);
    }

    const handleOnChange = (name, value) => {
        if (name == 'password') {
            setPwd(value)
        }
        setFrmState({ ...state, [name]: value, empNo: value })
    }

    const handleOnEdit = (n) => {
        toggle();
        setPayMonthId(n.id)
    }

    const handlePayroll = async (n) => {
        await PayMonthService.EmpPaySlip(n.employeeID, n.payMonthId).then(res => {
            setPaySlip(res.data);
        }).catch(err => {
            formUtil.displayErrors(err);
        })
        setPayMonthId(n.payMonthId);
        setEmployeeID(n.employeeID);
        togglePayslip();
    }

    const handleAnnexure = async (n) => {
        let incentivesAndPayCut = {};
        await IncentivesPayCutService.getIncetivesPayCutPayRoll(n.employeeID, props.payRoll.month, props.payRoll.year).then((res) => {
            incentivesAndPayCut = res.data;
        }).catch((err) => {
            notifyError(err.message);
        });
        setIncentivesAndPayCut(incentivesAndPayCut);
        toggleAnnexure();
    }
    const downloadPaySlip = async (n) => {
        await PayMonthService.DownloadPaySlip(n.employeeID, n.payMonthId).then((r) => {
            FileDownload(r.data, `PaySlip.pdf`, r.headers['content-type']);
            notifySaved("Downloaded successfully")
        }).catch(err => {
            formUtil.displayErrors(err);
        })
    }
    const handleHold = async () => {
        await PayMonthService.getHoldSalary(payMonthId).then(res => {
        }).catch(err => {
            formUtil.displayErrors(err);
        })
        window.location.reload()
        toggle();
    }

    const paySheetFile = async (e) => {
        await setPwd('')
        await setDModal(true)
    }
    const handleDownload = async () => {
        setLoading(true)
        await PayMonthService.PaySheetDownload(props.rid, pwd).then((r) => {
            FileDownload(r.data, `$PaySheet.xlsx`, r.headers['content-type']);
            notifySaved("Downloaded successfully")
            setDModal(!dModal)
        }).catch(err => {
            formUtil.displayErrors(err);
        })
        setLoading(false)
    }
    const dToggle = () => setDModal(!dModal)
    return (
        <Fragment>
            {
                state.isLoading ? <Loading /> :
                    <Fragment>
                        <Row>
                            <Col xs='3'>
                                <RWDropdownList {...{
                                    name: 'filters', label: 'Filters', valueField: 'id', textField: 'filter',
                                    value: state.filters, type: 'string', values: []
                                }} handlevaluechange={handleOnChange} />
                            </Col>
                            <Col xs='3'>
                                <div className="input-icon mr-3">
                                    <Label>Search Name</Label>
                                    <input name="name" className="form-control form-control-solid bg-white" placeholder="Search Name" onChange={(e) => handleOnChange(e.target.name, e.target.value)} />
                                    <span onClick={(e) => handleSearch(0)}>
                                        <i className="pe-7s-search text-muted mt-4"></i>
                                    </span>
                                </div>
                            </Col>

                            <Col xs='6' className='d-flex flex-row-reverse'>
                                <FontAwesomeIcon icon={faDownload} size='2x' onClick={() => paySheetFile()} />
                            </Col>
                        </Row>
                        <Row>
                            <Modal isOpen={dModal} toggle={dToggle}>
                                <ModalBody>
                                    <Col ms='4'>
                                        <Input {...{
                                            name: 'password', label: 'Sheet Password',
                                            error: false, touched: false, type: 'password',
                                            value: pwd
                                        }} handlevaluechange={handleOnChange} />
                                    </Col>
                                    <Button style={{ marginLeft: '20px' }} type='button'
                                        disabled={(state.password == '' || loading == true) ? true : false} onClick={() => handleDownload()}>{(state.password == '' && loading == true) ? 'Please Wait' : 'Download'}</Button>
                                </ModalBody>
                            </Modal>
                        </Row>
                        <Table handleSearch={handleSearch} columns={columns} data={state.data} pages={state.pages} hasPrevious={state.hasPrevious} hasNext={state.hasNext} pageIndex={state.pageIndex} showPaginate={true} handleOnEdit={handleOnEdit} />

                    </Fragment>
            }
            <Modal isOpen={modal} toggle={toggle} >
                <ModalBody>
                </ModalBody>
                <h3 className='text-center'>Are you sure to hold?</h3>
                <ModalFooter>
                    <Button color="primary" onClick={() => handleHold()}>Submit</Button>{' '}
                    <Button color="secondary" onClick={toggle}>Cancel</Button>
                </ModalFooter>
            </Modal>

            <Modal isOpen={payslipModal} toggle={togglePayslip} >
                <ModalBody>
                    <SalaryDetails paySlip={paySlip} />
                </ModalBody>
            </Modal>
            <Modal isOpen={annexureModal} toggle={toggleAnnexure} >
                <ModalBody>
                    <Annexure incentivesAndPayCut={incentivesAndPayCut}/>
                </ModalBody>
            </Modal>
        </Fragment>
    )
}

export default Index
