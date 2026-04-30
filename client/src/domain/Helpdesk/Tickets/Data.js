import PageHeader from "Layout/AppMain/PageHeader"
import RichTextArea from "components/dynamicform/Controls/RichTextArea"
import InfoDisplay from "domain/Approvals/InfoDisplay"
import React, { Fragment } from "react"
import { CardBody, Col, Row } from "reactstrap"
import * as dateUtil from 'utils/date'

export const ResponseInfo = (props) => {
    const { info, response, handleValueChange } = props
    return <Fragment>
        <PageHeader title={`Edit The Response of Ticket No: ${info.no}`} />
        <CardBody>
            <Row>
                <Col md='6'>
                    <InfoDisplay label="Status" info={info.status} />
                    <InfoDisplay label="Raised By" info={info.repliedBy} />
                    <InfoDisplay label="Raised On" info={dateUtil.DisplayDateTime(info.raisedOn)} />
                </Col>
                <Col>
                    <InfoDisplay label="Last Replied By" info={info.repliedBy} />
                    <InfoDisplay label="Last Respond On" info={dateUtil.DisplayDateTime(info.repliedOn)} />
                </Col>
            </Row>
            <Row className='mb-2'>
                <Col>
                    <RichTextArea {...{
                        name: 'response', label: 'Response',
                        value: response,
                        height: '200px',
                    }} handlevaluechange={handleValueChange} />
                </Col>
            </Row>
        </CardBody>
    </Fragment>
}