import { faEdit } from '@fortawesome/fontawesome-free-solid';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import React, { Fragment } from 'react';
import PerfectScrollbar from 'react-perfect-scrollbar';
import { VerticalTimeline, VerticalTimelineElement } from "react-vertical-timeline-component";
import { Col, Label, Row } from 'reactstrap';
import * as dateUtil from 'utils/date';
import queryString from 'query-string';
import * as crypto from 'utils/Crypto';

export default function TimeLine(props) {
    const { isEditable = false, messageReceive, comments, path } = props
    const handleEdit = async (id) => {
        if (messageReceive) {
            window.addEventListener("storage", messageReceive);
        }
        const qry = { r: (id ? crypto.encrypt(id) : '') };
        if (path) {
            window.open(path + queryString.stringify(qry));
        } 
    }
    return <>
        <Row>
            <Col md='12'>
                <div className="scroll-area-lg">
                    <PerfectScrollbar>
                        <Label><h6 className='text-info'>Response</h6></Label>

                        <VerticalTimeline layout="1-column" style={{ marginTop: '10px' }}>
                            {comments.map((i, j) => {
                                return <Fragment key={j}>
                                    <VerticalTimelineElement
                                        className="vertical-timeline-item"
                                        icon={<i className="badge " > </i>}
                                        date={<b className='text-primary'>{dateUtil.getExpireDate(i.repliedOn)}</b>}
                                    >
                                        <h6 className='text-danger'>{i.repliedBy}</h6>
                                        <div dangerouslySetInnerHTML={{ __html: i.response }} />
                                        {isEditable && <FontAwesomeIcon icon={faEdit} style={{ color: 'green' }} onClick={() => handleEdit(i.id)} />}
                                    </VerticalTimelineElement>
                                </Fragment>
                            })}
                        </VerticalTimeline>
                    </PerfectScrollbar>
                </div>
            </Col>
        </Row>
    </>
}