import { Injectable } from '@angular/core';
import { MessageService } from 'primeng/api';
import { AppService } from './app.service';

@Injectable({
  providedIn: 'root',
})
export class AlertService {
  constructor(
    private messageService: MessageService,
    private appService: AppService
  ) {}

  showMessage(message: string, type: alertType) {
    this.messageService.add({
      summary: this.getHeader(type),
      detail: message,
      severity: this.getAlertType(type),
    });
  }

  showDefaultMessage(messageType: defaultMessageType, type: alertType) {
    this.messageService.add({
      summary: this.getHeader(type),
      severity: this.getAlertType(type),
      detail: this.getDefaultMessage(messageType),
    });
  }

  private getAlertType(type: alertType) {
    switch (type) {
      case alertType.success:
        return 'success';
      case alertType.info:
        return 'info';
      case alertType.error:
        return 'error';
      case alertType.warn:
        return 'warn';
      case alertType.contrast:
        return 'contrast';
      case alertType.secondary:
        return 'secondary';
    }
  }

  private getHeader(type: alertType): string {
    switch (type) {
      case alertType.error:
        return this.appService.getTranslate('Error');
      case alertType.info:
        return this.appService.getTranslate('Information');
      case alertType.success:
        return this.appService.getTranslate('Success');
      case alertType.warn:
        return this.appService.getTranslate('Warning');
      case alertType.contrast:
        return this.appService.getTranslate('Contrast');
      case alertType.secondary:
        return this.appService.getTranslate('Secondary');
    }
  }

  private getDefaultMessage(type: defaultMessageType): string {
    switch (type) {
      case defaultMessageType.save:
        return 'Kaydetme işlemi başarıyla gerçekleşti.';
      case defaultMessageType.update:
        return 'Güncelleme işlemi başarıyla gerçekleşti.';
      case defaultMessageType.delete:
        return 'Silme işlemi başarıyla gerçekleşti.';
      case defaultMessageType.error:
        return 'Beklenmeyen bir hata oluştu.';
    }
  }
}

export enum alertType {
  success,
  info,
  error,
  warn,
  contrast,
  secondary,
}

export enum defaultMessageType {
  save,
  update,
  delete,
  error,
}
