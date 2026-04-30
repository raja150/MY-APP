<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:template match="/">
    <html >
      <style>

        * { margin:0px 0px; padding:0px 0px; box-sizing: border-box;  }
        body { font-family: 'Roboto', sans-serif; width: 830px; margin: 0 auto; font-size: 14px; }

        table {
        font-family: arial, sans-serif;
        border-collapse: collapse;
        width: 100%;
        table-layout: fixed;
        }
        table td { vertical-align: top; }
        table td, th {  border: 1px solid #464646;
        text-align: left;
        padding: 3px 3px 3px 12px; font-size: 14px; }

        #table1 td { width: 50%; }

        #table2 { margin-top: 50px; }

        #table3 { margin-top: 50px; }
        #table3 ul { list-style: none; line-height: 1.8; }

        #table4 { margin-top: 30px; }
        #table4 ul { list-style: none; line-height: 1.8; }

        #table5 { margin-top: 30px; }
        #table6 { margin-top: 10px; }
        .borderless td { border-top: 0; border-bottom: 0; }
        .topborderless td { border-bottom: 0; }
        .bottomborderless td { border-top: 0; }


        table.borderlesschaild tr td {
        border: none;
        padding-left: 0;
        }

        .watermark{
        margin-top: 30px;
        }
        .center {
        display: block;
        margin-left: auto;
        margin-right: auto;
        width: 240px;
        height:60px;
        }
        .address {
        margin-top: 5px;
        vertical-align: top;
        display: inline-block;
        text-align: center;
        width: auto;
        margin-left:260px;
        }
        .alignment{
        justify-content:space-between;
        display:flex;
        }

      </style>
      <body>
        <img src="https://avontix.com/wp-content/uploads/2023/07/Avontix-logo.svg" class='center'/>
        <div class='address'>
          <h4 style="font-weight: 500;">
            <xsl:value-of select="PaySlipPdfModel/Address/Address1"/>,
          </h4>

          <br/>
          <h4 style="margin-top: -13px; font-weight: 500;">
            <xsl:value-of select="PaySlipPdfModel/Address/Address2"/>.
          </h4>
        </div>
        <div class="watermark">
          <h4 style="margin-bottom: 30px; font-weight: 500;">
            Salary Statement for the month of : <xsl:value-of select="PaySlipPdfModel/Month"/>
          </h4>
          <div id="table1">
            <table style="width: 100%;">
              <tr >
                <tr  class="topborderless">
                  <td style="text-align: left;" colspan="2">Employee No</td>
                  <td style="text-align: left;" colspan="1">
                    <xsl:value-of select="PaySlipPdfModel/No"/>
                  </td>
                  <td style="text-align: left;" colspan="2">PF Number</td>
                  <td style="text-align: left;" colspan="1">
                    <xsl:value-of select="PaySlipPdfModel/EPFNo"/>
                  </td>

                </tr>
                <tr  class="borderless">
                  <td style="text-align: left;" colspan="2">Employee Name</td>
                  <td style="text-align: left;" colspan="1">
                    <xsl:value-of select="PaySlipPdfModel/Name"/>
                  </td>
                  <td style="text-align: left;" colspan="2">ESI Number</td>
                  <td style="text-align: left;" colspan="1">
                    <xsl:value-of select="PaySlipPdfModel/ESINo"/>
                  </td>
                </tr>
                <tr  class="borderless" >
                  <td style="text-align: left;" colspan="2">Designation </td>
                  <td style="text-align: left;" colspan="1">
                    <xsl:value-of select="PaySlipPdfModel/Designation"/>
                  </td>
                  <td style="text-align: left;" colspan="2">Bank Name</td>
                  <td style="text-align: left;" colspan="1">
                    <xsl:value-of select="PaySlipPdfModel/BankName"/>
                  </td>
                </tr>
                <tr  class="borderless">
                  <td style="text-align: left;" colspan="2">Department </td>
                  <td style="text-align: left;" colspan="1">
                    <xsl:value-of select="PaySlipPdfModel/Department"/>
                  </td>
                  <td style="text-align: left;" colspan="2">Account Number</td>
                  <td style="text-align: left;" colspan="1">
                    <xsl:value-of select="PaySlipPdfModel/BankACNo"/>
                  </td>
                </tr>
                <tr  class="borderless" >
                  <td style="text-align: left;" colspan="2">No.Of Working Days  </td>
                  <td style="text-align: left;" colspan="1">
                    <xsl:value-of select="PaySlipPdfModel/WorkDays"/>
                  </td>
                  <td style="text-align: left;" colspan="2">PAN </td>
                  <td style="text-align: left;" colspan="1">
                    <xsl:value-of select="PaySlipPdfModel/PAN"/>
                  </td>
                </tr>
                <tr   class="bottomborderless">
                  <td style="text-align: left;" colspan="2">No.of Present Days </td>
                  <td style="text-align: left;" colspan="1">
                    <xsl:value-of select="PaySlipPdfModel/PresentDays"/>
                  </td>
                  <td style="text-align: left;" colspan="2">LOP </td>
                  <td style="text-align: left;" colspan="1">
                    <xsl:value-of select="PaySlipPdfModel/LOP"/>
                  </td>
                </tr>
              </tr>
            </table>
          </div>
          <div id="table3">
            <table>
              <tr>
                <th style="text-align: center;" colspan="6">Salary Details</th>
              </tr>
              <tr>
                <td style="text-align: left;" colspan="2">
                  <strong>Earnings</strong>
                </td>
                <td style="text-align: left;" colspan="1">
                  <strong>Amount</strong>
                </td>
                <td style="text-align: left;" colspan="2">
                  <strong>Deductions</strong>
                </td>
                <td style="text-align: left;" colspan="1">
                  <strong>Amount</strong>
                </td>
              </tr>
              <xsl:for-each select="PaySlipPdfModel/Components/PaySlipPdfComponentsModel">
                <tr class="borderless">
                  <td style="text-align: left;" colspan="2">
                    <xsl:value-of select="Earning" />
                  </td>
                  <td style="text-align: right;" colspan="1">
                    <xsl:value-of select="EarningAmount"/>
                  </td>
                  <td style="text-align: left;" colspan="2">
                    <xsl:value-of select="Deduction" />
                  </td>
                  <td style="text-align: right;" colspan="1">
                    <xsl:value-of select="DeductionAmount"/>
                  </td>
                </tr>
              </xsl:for-each>

              <tr>
                <td style="text-align: left;" colspan="2">Gross Earnings</td>
                <td style="text-align: right;" colspan="1">
                  <xsl:value-of select="PaySlipPdfModel/Gross"/>
                </td>
                <td style="text-align: left;" colspan="2">Gross Deductions</td>
                <td style="text-align: right;" colspan="1">
                  <xsl:value-of select="PaySlipPdfModel/Deduction"/>
                </td>
              </tr>
            </table>
          </div>

          <p style="margin-top: 20px;">
            <strong>
              Net Payable: Rs.<xsl:value-of select="NetPayble"/>
            </strong><xsl:value-of select="PaySlipPdfModel/Net"/>/-
          </p>
          <p>
            Bank Ac.No.: <xsl:value-of select="PaySlipPdfModel/BankACNo"/>
          </p>

          <p style="margin-top: 20px;">
            <strong>Note: </strong>This is computer generated pay slip and does not require signature.
          </p>
        </div>
      </body>
    </html>

  </xsl:template>
</xsl:stylesheet>

