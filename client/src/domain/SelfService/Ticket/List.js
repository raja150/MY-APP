import React, { useEffect, useState } from 'react';
import { Button, Card, CardBody, Col, Input, Label, Row } from 'reactstrap';
import TicketService from 'services/SelfService/Ticket';
import DataTable from '../../../components/table/DataTable';
import UpdateTable from '../../HOC/withDataTable';
import { RWDropdownList } from 'components/dynamicform/Controls';
import TicketStsService from 'services/HelpDesk/TicketSts';
import Loading from 'components/Loader';
import * as _ from 'lodash'

const fields = [
    { name: 'no', label: 'Ticket No', type: 'input' },
    { name: 'raisedOn', label: 'Raised On', type: 'datetime' },
    { name: 'subject', label: 'Subject', type: 'input' },
    { name: 'department', label: 'Department', type: 'input' },
    { name: 'status', label: 'Status', type: 'input' },
    { name: 'helpTopic', label: 'Help Topic', type: 'input' },
];
const title = 'Raised Tickets';

export const STATUS = [{ value: 1, text: "Open" }, { value: 3, text: "Closed" }, { value: 4, text: 'In process' }]

function RaiseTicketList(props) {
    const [state, setState] = useState({ isLoading: true, tickets: [] })
    useEffect(async () => {
        await TicketStsService.getList().then((res) => {
            let tts = _.orderBy(res.data, ['orderNo'], ['asc'])
            setState({ ...state, tickets: tts, isLoading: false })
        })
    }, [])
    const searchCard = () => {
        return (
            <Card className='mb-1'>
                <CardBody>
                    <Row>
                        <Col md='2'>
                            <RWDropdownList {...{
                                name: 'ticketStsId', label: 'Status', valueField: 'id', textField: 'name',
                                value: props.searchData['ticketStsId'],
                                values: state.tickets
                            }}
                                handlevaluechange={(e, v) => props.handleOnChange('ticketStsId', v)}
                            />
                        </Col>
                        <Col md="1" className='mt-3'>
                            <Button className="mr-2  btn-icon btn-icon-only btn-secondary btn-sm" onClick={(e) => props.handleSearch(0)}>
                                <i className="pe-7s-search btn-icon-wrapper"> </i></Button>
                        </Col>
                    </Row>
                </CardBody>
            </Card>
        )
    }
    return (
        state.isLoading ? <Loading /> : <DataTable {...props} searchCard={searchCard} />
    )

}
export default UpdateTable(RaiseTicketList, TicketService.paginate(), fields, '', title, '', '')