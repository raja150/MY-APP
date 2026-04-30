import { faDownload, faCalculator } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { notifyError, notifySaved } from 'components/alert/Toast';
import { FileDownload } from 'domain/Setup/DataImport/file-download';
import * as _ from 'lodash';
import React, { useEffect, useState } from 'react';
import { Button, Card, CardBody, Col, Input, Label, Modal, ModalBody, Row, ModalHeader, HeaderComponent } from 'reactstrap';
import DataTable from '../../../components/table/DataTable';
import { default as RentalService, default as TaxDeclarationService } from '../../../services/PayRollServices/TaxDeclarationService';
import UpdateTable from '../../HOC/withDataTable';
import { EmployeeImageCell, EmployeeImageProps } from 'domain/Organization/ImageDetails/ConstImage';
import { Radio } from 'components/dynamicform/Controls';
import { WithOutSymbol } from 'components/Formats/MoneyFormat'
import EmployeeDetails from 'services/List/EmployeeDetails';
import { Saving } from '../../../components/Loader';

const fields = [
    { ...EmployeeImageProps, Cell: ({ row }) => EmployeeImageCell(row.original.employeeId) },
    {
        name: 'employeeId', label: 'Employee', type: 'custom', width: 150,
        Cell: ({ value, row }) => {
            const currentRow = row.original
            return (
                <EmployeeDetails currentRow={currentRow} />
            )
        }
    },
    { name: 'dateOfJoining', label: 'Date of joining', type: 'date' },
    { name: 'year', label: 'Year', type: 'input' },
    {
        name: 'taxable', label: 'Taxable Income', type: 'custom',
        Cell: ({ value }) => {
            return <WithOutSymbol value={value} />;
        }
    },
    {
        name: 'tax', label: 'Tax', type: 'custom',
        Cell: ({ value }) => {
            return <WithOutSymbol value={value} />;
        }
    },
    {
        name: 'due', label: 'Due', type: 'custom',
        Cell: ({ value }) => {
            return <WithOutSymbol value={value} />;
        }
    },
    {
        name: 'isNewRegime', label: 'New/Old Regime ', type: 'custom',
        Cell: ({ value }) => {
            return (
                <div>
                    {!value ? "Old Regime" : "New Regime"}
                </div>
            )
        }
    }]


const title = 'Employee Tax Information'

function DeclarationList(props) {
    const [state, setState] = useState({ isLoading: true, financialYear: [], financialYearId: '', });
    const [dModal, setDModal] = useState(false);
    const [isLoading, setIsLoading] = useState(true);
    const [calcForAll, setCalcForAll] = useState(false);
    const [isDownloading, setIsDownloading] = useState(false);
    useEffect(() => {
        setIsLoading(true)
        const fetchData = async () => {
            let financialYear = []
            await TaxDeclarationService.getFinancialYear().then((result) => {
                financialYear = result.data;
            })
            setState({ ...state, financialYear: financialYear })
            setIsLoading(false);
        }
        fetchData();
    }, [])
    const paySheetFile1 = async (e) => {
        setCalcForAll(true);
        await TaxDeclarationService.CalculateForAll(props.searchData.financialYearId).then((r) => {
            notifySaved("Successfully completed.")
        }).catch(e => {
            notifyError(e.message)
        })
        setCalcForAll(false);
    }
    const paySheetFile = async (e) => {
        await setDModal(true)
    }
    const handleDownload = async () => {
        setIsDownloading(true);
        await TaxDeclarationService.DeclarationDownload(props.searchData.financialYearId, props.searchData.password).then((r) => {
            FileDownload(r.data, `$Declaration.xlsx`, r.headers['content-type']);
            notifySaved("Downloaded Successfully")
            setDModal(!dModal)
        }).catch(e => {
            notifyError(e.message)
        })
        setIsDownloading(false);
    }
    const dToggle = () => setDModal(!dModal)
    const searchCard = () => {
        return (
            <Card className='mb-2'>
                <CardBody>
                    <Row>
                        <Col md='2'>
                            <Label>Financial Year</Label>
                            <Input name='financialYearId' type='select' className="form-control form-control-sm"
                                value={props.searchData['financialYearId']}
                                onChange={(e) => props.handleOnChange(e.target.name, e.target.value)} >
                                <option>Select</option>
                                {state.financialYear.map((item, k) => {
                                    return <option key={k} value={item.id}>{item.name}</option>
                                })}
                            </Input>
                        </Col>
                        <Col md='3'>
                            <Label>Employee </Label>
                            <Input name='name' className="form-control form-control-sm"
                                value={props.searchData['name']}
                                onChange={(e) => { props.handleOnChange(e.target.name, e.target.value) }}
                                onKeyPress={(e) => { e.key === "Enter" && props.handleSearch(0) }}></Input>
                        </Col>
                        <Col md='3'>
                            <Radio {...{
                                name: 'taxPayers', value: props.searchData.taxPayers ? props.searchData.taxPayers : false,
                                values: [{ value: '', text: 'All' }, { value: 1, text: 'Tax Payers' }, { value: 2, text: 'Tax Due' }],
                            }} handlevaluechange={(name, value) => props.handleOnChange(name, value)} />
                        </Col>
                        <Col md='1' className='mt-3'>
                            {!calcForAll ? <Button className="mr-2  btn-icon btn-icon-only btn-secondary btn-sm" onClick={(e) => props.handleSearch(0)}>
                                <i className="pe-7s-search btn-icon-wrapper"> </i></Button> : ''}
                        </Col>
                        <Col md='1' className='mt-3'>
                            {!_.isEmpty(props.searchData) && props.searchData.financialYearId
                                && props.searchData.financialYearId !== 'Select'
                                && !calcForAll ?
                                <FontAwesomeIcon icon={faDownload} size='2x' style={{ cursor: "pointer" }} onClick={() => paySheetFile()} />
                                : ''}
                        </Col>
                        <Col md='1' className='mt-3'>
                            {!_.isEmpty(props.searchData) && props.searchData.financialYearId && props.searchData.financialYearId !== 'Select' ?
                                calcForAll ?
                                    <Saving /> :
                                    <FontAwesomeIcon icon={faCalculator} size='2x' style={{ cursor: "pointer" }} onClick={() => paySheetFile1()} />
                                : ''}
                        </Col>
                    </Row>
                    <Row>
                        <Modal isOpen={dModal} toggle={dToggle}>
                            <ModalHeader><h5 >Password for excel file</h5></ModalHeader>
                            <ModalBody>
                                <Row>
                                    <Col ms='12' className='mt-3'>
                                        <Label>Sheet Password</Label>
                                        <Input name='pwdDownload' type='password'
                                            className="form-control form-control-sm"
                                            onChange={(e) => {
                                                props.handleOnChange('password', e.target.value);
                                            }}
                                            autoComplete="off" />
                                    </Col>
                                </Row>
                                <Row>
                                    <Col ms='12' className='mt-3 '>
                                        {!_.isEmpty(props.searchData) && props.searchData.password ?
                                            <Button style={{ marginLeft: '20px' }} type='button'
                                                disabled={isDownloading}
                                                onClick={() => handleDownload()}>
                                                {isDownloading ? 'Please Wait' : 'Download'}</Button>
                                            : ''}
                                    </Col>
                                </Row>
                            </ModalBody>
                        </Modal>
                    </Row>
                </CardBody>
            </Card>
        )
    }
    return (
        <DataTable {...props} searchCard={searchCard} tableLoading={calcForAll} />
    )
}
export default UpdateTable(DeclarationList, RentalService.paginate(), fields, '', title, '', '', '')
