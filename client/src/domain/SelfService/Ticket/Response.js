import RichTextArea from "components/dynamicform/Controls/RichTextArea";
import React, { Fragment, useState } from "react";
import { Button, Col, Row } from "reactstrap";
import TicketService from "services/SelfService/Ticket";
import { RAISE_TICKET } from "../navigation";
import * as formUtil from 'utils/form'
import { notifySaved } from "components/alert/Toast";

export default function UserResponse(props) {
    const { rid } = props
    const [msg, setMsg] = useState('')
    const [loading, setLoading] = useState(false)

    const handleValueChange = (name, value) => {
        setMsg(value)
    }
    const handleUserResponse = async () => {
        setLoading(true)
        const data = {
            ticketId: rid,
            response: msg
        }
        await TicketService.UserResponse(data).then((res) => {
            notifySaved();
            props.history.push(RAISE_TICKET);
        }).catch((error) => {
            formUtil.displayErrors(error)
        })
        setLoading(false)

    }
    return <Fragment>
        <Row className='mb-2'>
            <Col md='12'>
                <RichTextArea {...{
                    name: 'response', label: 'Response',
                    value: msg,
                    height: '200px',
                }} handlevaluechange={handleValueChange} />
            </Col>
            <Button className="ml-3 btn-icon" key='button'
                color="primary" type='button'
                disabled={loading}
                onClick={() => handleUserResponse()} >
                {loading ? "Please Wait..." : "Response"}
            </Button>
        </Row>
    </Fragment>
}