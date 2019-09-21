import { ResponseModel } from './response';
import * as momentJalaali from 'moment-jalaali';

class StatementInfo {
  statementSerialNo: number;
  statementType: number;
  date: string;
  time: string;
  statementBarcode: string;
  cardToken: string;
}

class SellerInfo {
  name: string;
  nationalCode: number;
  economicalNumber: number;
  registerNumber: number;
  invoiceNumber: string;
  postalCode: string;
  province: string;
  city: string;
  address: string;
  cityCode: string;
  telephoneNumber: string;
  fundNumber: string;
  fundName: string;
  customerSubscriptionCode: string;
  email: string;
  webSite: string;
}

class PaymentInfo {
  paymentType: number;
  paymentId: string;
  billId: string;
  issuerBankId: number;
  amount: number;
  status: boolean;
  rrn: string;
  stan: string;
  resultDescription: string;
  actualBalance: number;
  availableBalance: number;
  terminalType: number;
  acquireBankId: number;
  merchantNo: string;
  terminalNo: string;
  acquirePspName: string;
  targetCardNo: string;
  targetAccountNo: string;
  targetAccountName: string;
  dynamicPinRegisterCode: string;
  discountAmount: number;
  chargeType: number;
  chargeSerialNo: string;
  chargePin: string;
  mobileNo: string;
  billType: number;
  observerCompany: string;
  callCenterTelephoneNumber: string;
  installationLocation: number;
}

export class Receipt {
  statementInfo: StatementInfo;
  sellerInfo: SellerInfo;
  paymentInfo: PaymentInfo;
  lstInvoiceDetails: string;
  InviocePaymentInfo: string;
}

export class ReceiptList {
  receipts: Receipt[];
  date: momentJalaali.Moment;
}

export class ReceiptRequest {
  cardno: string;
  startDate: string;
  endDate: string;
}

export class ReceiptResponse extends ResponseModel {
  receipts: Receipt[];
}
