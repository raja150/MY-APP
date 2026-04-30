import Table from 'domain/PayRuns/PayRun/table';
import React, { useEffect, useState } from 'react';
import { Button, CardHeader, Col, Label, Modal, ModalBody, Row } from 'reactstrap';
import { faEdit, faTrash } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { notifySaved } from 'components/alert/Toast';
import AddContact from './AddContact';
import * as _ from 'lodash'
import ProfileService from 'services/Org/ProfileService';
import UpdateProfileService from 'services/Org/UpdateProfileService';

export default function ContactList(props) {
    const [rowData, setRowData] = useState({})
    const [modal, setModal] = useState(false)
    const[deletedId , setDeletedId]= useState('');

    const Columns = [
        {
            Header: 'Person Name',
            accessor: 'personName'
        },
        {
            Header: 'Relationship',
            accessor: 'humanRelation',
            Cell: ({ value }) => (
                <div>{value == 1 ? "Father" : value == 2 ? 'Mother'
                    : value == 3 ? 'Spouse' : value == 4 ? 'Child 1' : value == 5 ? 'Child 2' : value == 6 ? 'Child 3' : '-'}</div>
            )
        },
        {
            Header: 'Contact Number',
            accessor: 'contactNo'
        },
        // {
        //     Header: 'Edit',
        //     accessor: 'edit', Cell: ({ value, row }) => <FontAwesomeIcon icon={faEdit} style={{ color: 'green' }} onClick={() => handleOnEdit(row.original)} />

        // },
        // {
        //     Header: 'Delete',
        //     accessor: 'delete', Cell: ({ value, row }) => <FontAwesomeIcon icon={faTrash} style={{ color: 'red' }} onClick={() =>handleDeleteId(row.original) } />
        // }

    ]

    const [state, setState] = useState({ isLoading: true, contactDetails: [...props.data] })

    const [addContactModal, setAddContactModal] = useState(false)
    const addContactToggle = () => {
        setAddContactModal(!addContactModal)
    }

    const fetchData = async () => {
        let contactDetails = {}
        await ProfileService.getContact().then((response) => {
            contactDetails = response.data
            setState({ ...state, isLoading: false, contactDetails: contactDetails })
        })
    }

    useEffect(() => {
        fetchData();
    }, [])

    const handleOnEdit = (row) => {
        setRowData(row);
        setAddContactModal(true)
    }

    const handleDeleteId = async (n) => {
        setDeletedId(n.id)
        setModal(!modal)
    }
    const handleDelete = async (id) => {
        await UpdateProfileService.DeleteContact(id).then((res) => {
            notifySaved('Deleted Successfully');
            setModal(!modal)
            fetchData();
        })
    }

    return <>

        <CardHeader>
            Contact Details

            {/* <Button className="btn-actions-pane-right" color="primary" type="button"
                onClick={() => setAddContactModal(true)}
            >
                <i className="pe-7s-plus btn-icon-wrapper font-weight-bold" />
                ADD
            </Button> */}
        </CardHeader>
        <Table columns={Columns} data={state.contactDetails} handleOnEdit={handleOnEdit} showPaginate={false} />
        {addContactModal && <AddContact modal={addContactModal} toggle={addContactToggle} setRowData={setRowData} data={state.contactDetails} rowData={rowData} fetchdata={fetchData} />}

        <Modal isOpen={modal}>
            <ModalBody>
                <Col ms="4">
                    <Label><h6>Are you sure do you want to delete contact details ?</h6></Label>
                </Col>
                <Row>
                    <div style={{ paddingLeft: '30px' }} >
                        <Button type="button" color="success" onClick={() => handleDelete(deletedId)}>
                            Yes
                        </Button>

                    </div>
                    <div style={{ paddingLeft: '10px' }} >
                        <Button color="danger" type="button" onClick={() => setModal(!modal)} >
                            No
                        </Button>
                    </div>
                </Row>
            </ModalBody>
        </Modal>

    </>
}