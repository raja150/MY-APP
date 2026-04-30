
import React from 'react';
import { Card, Col, Progress, Row } from 'reactstrap';
 

const Appointments = (props) => {
    const categoryData = props.data.filter((k) => k.type === 10);
    const categoryJson = categoryData.length > 0 ? JSON.parse(categoryData[0].json) : [];

    const categoryFalse = categoryJson && categoryJson.find((k) => k.Category === false);
    const categoryTrue = categoryJson && categoryJson.find((k) => k.Category === true);

    const statusData = props.data.filter((k) => k.type === 11);
    const statusJson = statusData.length > 0 ? JSON.parse(statusData[0].json) : [];
    
    const cancelSts = statusJson && statusJson.find((k) => k.Status === 0);
    const reSchSts = statusJson && statusJson.find((k) => k.Status === 5);
    const noshowSts = statusJson && statusJson.find((k) => k.Status === 6);
    
    return (
        <Card className="main-card p-2">
            <h6>Appointments</h6>
            {/* <div
                className="widget-chart-wrapper widget-chart-wrapper-lg opacity-10 m-0">
                <ResponsiveContainer width='100%' aspect={3.0 / 1.0}>
                    <LineChart data={data55}
                        margin={{ top: 0, right: 5, left: 5, bottom: 0 }}>
                        <Tooltip />
                        <Line type="monotone" dataKey="pv" stroke="#d6b5ff"
                            strokeWidth={2} />
                        <Line type="monotone" dataKey="uv" stroke="#a75fff"
                            strokeWidth={2} />
                    </LineChart>
                </ResponsiveContainer>
            </div> */}
            <Row>
                <Col md="12" lg="2">
                    <div className="widget-content">
                        <div className="widget-content-outer">
                            <div className="widget-content-wrapper">
                                <div className="widget-content-left">
                                    <div className="widget-numbers text-dark">
                                    {(categoryFalse && categoryFalse.Count) || 0}
                                    </div>
                                </div>
                            </div>
                            <div className="widget-progress-wrapper mt-1">
                                <Progress
                                    className="progress-bar-xs progress-bar-animated-alt"
                                    color="info"
                                    value="65" />
                                <div className="progress-sub-label">
                                    <div className="sub-label-left font-size-md">
                                    New
                                                                </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </Col>
                <Col md="12" lg="2">
                    <div className="widget-content">
                        <div className="widget-content-outer">
                            <div className="widget-content-wrapper">
                                <div className="widget-content-left">
                                    <div className="widget-numbers text-dark">
                                    {(categoryTrue && categoryTrue.Count) || 0 }
                                    </div>
                                </div>
                            </div>
                            <div className="widget-progress-wrapper mt-1">
                                <Progress
                                    className="progress-bar-xs progress-bar-animated-alt"
                                    color="warning"
                                    value="22" />
                                <div className="progress-sub-label">
                                    <div className="sub-label-left font-size-md">
                                    Followup
                                                                </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </Col>
                <Col md="12" lg="2">
                    <div className="widget-content">
                        <div className="widget-content-outer">
                            <div className="widget-content-wrapper">
                                <div className="widget-content-left">
                                    <div className="widget-numbers text-dark">
                                    {(cancelSts && cancelSts.Count) || 0 }
                                    </div>
                                </div>
                            </div>
                            <div className="widget-progress-wrapper mt-1">
                                <Progress
                                    className="progress-bar-xs progress-bar-animated-alt"
                                    color="success"
                                    value="83" />
                                <div className="progress-sub-label">
                                    <div className="sub-label-left font-size-md">
                                    Canceled
                                                                </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </Col>
                <Col md="12" lg="2">
                    <div className="widget-content">
                        <div className="widget-content-outer">
                            <div className="widget-content-wrapper">
                                <div className="widget-content-left">
                                    <div className="widget-numbers text-dark">
                                    {(reSchSts && reSchSts.Count) || 0 }
                                    </div>
                                </div>
                            </div>
                            <div className="widget-progress-wrapper mt-1">
                                <Progress
                                    className="progress-bar-xs progress-bar-animated-alt"
                                    color="success"
                                    value="83" />
                                <div className="progress-sub-label">
                                    <div className="sub-label-left font-size-md">
                                    Noshow
                                                                </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </Col>
                <Col md="12" lg="2">
                    <div className="widget-content">
                        <div className="widget-content-outer">
                            <div className="widget-content-wrapper">
                                <div className="widget-content-left">
                                    <div className="widget-numbers text-dark">
                                    {(noshowSts && noshowSts.Count) || 0 }
                                    </div>
                                </div>
                            </div>
                            <div className="widget-progress-wrapper mt-1">
                                <Progress
                                    className="progress-bar-xs progress-bar-animated-alt"
                                    color="success"
                                    value="83" />
                                <div className="progress-sub-label">
                                    <div className="sub-label-left font-size-md">
                                    Rescheduled
                                                                </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </Col>
            </Row>
        </Card>
    )
}

export default Appointments;