export interface Message {
    id: number;
    senderid: number;
    senderUsername: string;
    senderPhotoUrl: string;
    recipientId: number;
    recipientUsername: string;
    content: string;
    dateRead?: Date;
    messageSent: string;
}