import React, { useState } from 'react';
import { Button, Card, CardBody, Col, Modal, ModalBody, Row } from 'reactstrap';
import DataTable from '../../../components/table/DataTable';
import SalaryService from '../../../services/PayRoll/Salary';
import UpdateTable from '../../HOC/withDataTable';
import { EmployeeImageCell, EmployeeImageProps } from 'domain/Organization/ImageDetails/ConstImage';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faDownload } from '@fortawesome/free-solid-svg-icons';
import { FileDownload } from 'domain/Setup/DataImport/file-download';
import { notifySaved } from 'components/alert/Toast';
import { Input } from 'components/dynamicform/Controls';
import * as formUtil from 'utils/form';

const fields = [
    { ...EmployeeImageProps, Cell: ({ row }) => EmployeeImageCell(row.original.employeeId) },
    { name: 'empNo', label: 'Employee No', type: 'input' },
    { name: 'empName', label: 'Employee Name', type: 'input' },
    { name: 'department', label: 'Department', type: 'input' },
    { name: 'designation', label: 'Designation', type: 'input' },
    { name: 'dateOfJoining', label: 'Date of joining', type: 'date' },
    { name: 'salary', label: 'Salary', type: 'input' },
    { name: 'mobileNo', label: 'MobileNo', type: 'input' },
    { name: 'modifiedOn', label: 'Last updated on', type: 'date' },
]

const title = 'Employee Salary Info'


function SalaryList(props) {
    const [state, setState] = useState({ password: "" })
    const [loading, setLoading] = useState(false);
    const [modal, setModal] = useState(false);
    const [pwd, setPwd] = useState('')
    const EmployeeSalaryDownload = async (e) => {
        setPwd('')
        setModal(true)
    }

    const handleOnChange = (name, value) => {
        if (name == 'sheetPassword') {
            setPwd(value)
        }
        setState({ ...state, password: value })
    }
    const handleDownload = async () => {
        setLoading(true)
        await SalaryService.EmployeeSalaryDownload(pwd).then((r) => {
            FileDownload(r.data, `$Employee Salary.xlsx`, r.headers['content-type']);
            notifySaved("Downloaded successfully")
            setModal(!modal)
        }).catch(err => {
            formUtil.displayErrors(err);
        })
        setLoading(false)
    }
    const toggle = () => setModal(!modal)
    const searchCard = () => {
        return (
            <Card>
                <CardBody>
                    <Row>
                        <Col md='3'>
                            <Input {...{
                                name: 'name', label: 'Employee Name/Employee No',
                                error: false, touched: false,
                                value: props.searchData['name']
                            }} handlevaluechange={(e, v) => props.handleOnChange(e, v)} />
                        </Col>
                        <Col md="3" className='mt-3'>
                            <Button className="mr-2 mt-1  btn-icon btn-icon-only btn-secondary btn-sm" onClick={(e) => props.handleSearch(0)}>
                                <i className="pe-7s-search btn-icon-wrapper"> </i></Button>
                        </Col>
                        <Col xs='6' className='d-flex flex-row-reverse'>
                            <FontAwesomeIcon icon={faDownload} size='2x' onClick={() => EmployeeSalaryDownload()} />
                        </Col>
                    </Row>
                    <Row>
                        <Modal isOpen={modal} toggle={toggle}>
                            <ModalBody>
                                <Col ms='4'>
                                    <Input {...{
                                        name: 'sheetPassword', label: 'Sheet Password',
                                        error: false, touched: false, type: 'password',
                                        value: pwd
                                    }} handlevaluechange={handleOnChange} />
                                </Col>
                                <Button style={{ marginLeft: '20px' }} type='button'
                                    disabled={(state.password == '') ? true : false} onClick={() => handleDownload()}>{(loading == true) ? 'Please Wait' : 'Download'}</Button>
                            </ModalBody>
                        </Modal>
                    </Row>
                </CardBody>
            </Card>
        )
    }
    return (
        <DataTable {...props} searchCard={searchCard} />
    )
}
export default UpdateTable(SalaryList, SalaryService.paginate(), fields, '', title, '', '', '')
