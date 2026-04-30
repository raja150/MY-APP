import React, { useState } from 'react';
import { Col, Label, Row, Modal, ModalBody, Button } from 'reactstrap';
import TableHeader from './TableHeader';
import { displayErrors } from 'utils/form';
import queryString from 'query-string';
import APIService from '../../services/apiservice';
import TableBody from './TableBody';

export default function ReportTable(props) {
    const [modal, setModal] = useState(false);
    const [popUpData, setPopUpData] = useState([]);
    const [popUpColumns, setPopUpColumns] = useState([]);

    const handlePopUp = async (rowData, column, popUpColumns, popUpFilters) => {
        let qryString = {}
        //pushing popUp filters related rowData into querystring object
        popUpFilters.forEach(element => {
            if (element in rowData) {
                qryString[element] = rowData[element]
            }
        });
        //Here setting required PopUp columns
        setPopUpColumns(popUpColumns)

        await APIService.getAsync(`${column.popUpUrl}?` + queryString.stringify(qryString)).then(res => {
            setPopUpData(res.data)
            setModal(!modal);
        }).catch(err => {
            displayErrors(err)
        })
    }

    return (
        <>
            {props.summaryColumns && props.summaryColumns.length > 0 ?
                <Row className='mt-2'>
                    <Col xs='12' className='table-responsive'>
                        <table className='table-bordered table-hover'>
                        {/* style={{borderCollapse:'collapse',marginBottom:'10px'}} */}
                        {/* className='table-bordered table-hover' */}
                            <TableHeader columns={props.summaryColumns} />
                            <TableBody columns={props.summaryColumns} data={props.summaryData} />
                        </table>
                    </Col>
                </Row>
                : ''}
            <Row className='mt-2'>
                <Col xs='12' className='table-responsive'>
                    <Label><strong>Total Records : {props.data.length}</strong></Label>
                    <table className='table table-bordered table-hover' id="table-to-xls" >
                        <TableHeader columns={props.columns} />
                        <TableBody columns={props.columns} data={props.data} handlePopUp={handlePopUp}
                            popUpFilters={props.popUpfilters} />
                    </table>
                </Col>
            </Row>
            {popUpColumns && popUpColumns.length > 0 && <Modal isOpen={modal}>
                <ModalBody>
                    <ReportTable columns={popUpColumns} data={popUpData} />
                    <Row>
                        <div style={{ paddingLeft: '150px' }} >
                            <Button color="primary" type="button" onClick={() => setModal(!modal)} >
                                Ok
                            </Button>
                        </div>
                    </Row>
                </ModalBody>
            </Modal>}
        </>
    );
}