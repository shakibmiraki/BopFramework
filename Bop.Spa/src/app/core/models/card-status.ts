import { ResponseModel } from './response';

export class CardStatusList extends ResponseModel {
  cards: CardStatus[];
}

export class CardStatus {
  pan: string;
  status: GetCardStatusResponse;
}

export class GetCardStatusResponse {
  cardExist: string;
  status: boolean;
}
