import InfoDisplay from "domain/Approvals/InfoDisplay"
import { Col, Row } from "reactstrap"
import React from "react"

const TicketInfo = (props) => {
    const { entityData } = props
    return <>
        {entityData.status &&
            <Row className='mb-4 ml-3'>
                <Col md='6'>
                    <InfoDisplay label="Department" info={
                        entityData.department} />
                    <InfoDisplay label="Help Topic" info={
                        entityData.helpTopic} />
                    <InfoDisplay label="Sub Topic" info={
                        entityData.subTopic} />
                    <InfoDisplay label="Issue Summary" info={
                        entityData.subject} />
                    <InfoDisplay label="Issue Details" info={
                        entityData.message} />
                </Col>
            </Row>
        }
    </>
}
export default TicketInfo