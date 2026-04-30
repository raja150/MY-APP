
import Loading from "components/Loader";
import { notifySaved } from "components/alert/Toast";
import queryString from 'query-string';
import React, { Fragment, useEffect, useState } from "react";
import { Button, Card, Row } from "reactstrap";
import TicketLogService from 'services/HelpDesk/TicketLog';
import SessionStorage from "services/SessionStorage";
import * as crypto from 'utils/Crypto';
import * as formUtil from 'utils/form';
import { ResponseInfo } from "./Data";

export default function EditResponse(props) {

    const parsed = queryString.parse(props.location.search);
    const rid = parsed.r ? crypto.decrypt(parsed.r) : null;
    const [state, setState] = useState({ isLoading: true, info: {} })
    const [response, setResponse] = useState('')

    useEffect(async () => {
        let info = {}
        await TicketLogService.getRespById(rid).then(res => {
            info = res.data;
            setResponse(info.response)
        })
        setState({ ...state, isLoading: false, info: info })
    }, [])
    
    const handleValueChange = async (name, value) => {
        setResponse(value)
    }
    const handleSubmit = async () => {
        const data = {
            id: rid,
            response: response
        }
        await TicketLogService.updateResponse(data).then(res => {
            notifySaved("Response updated successfully")
            setTimeout(() => {
                SessionStorage.setResponse()
                window.close()
            }, 2000);
        }).catch(er => {
            formUtil.displayAPIError(er)
        })
    }
    return <Fragment>
        {state.isLoading ? <Loading /> :
            <Card>
                <ResponseInfo
                    info={state.info}
                    handleValueChange={handleValueChange}
                    response={response}
                />
                <Row className="ml-4 mb-3">
                    <Button type="submit" color="btn btn-success" onClick={() => handleSubmit()}>Submit</Button>
                    <Button type="button" color="btn ml-2 btn-primary" onClick={() => window.close()}>Close</Button>
                </Row>

            </Card>
        }
    </Fragment>
}