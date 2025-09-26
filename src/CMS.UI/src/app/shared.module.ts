import { AvatarModule } from 'primeng/avatar';
import { MenubarModule } from 'primeng/menubar';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { TableModule } from 'primeng/table';
import { DialogModule } from 'primeng/dialog';
import { CheckboxModule } from 'primeng/checkbox';
import { ButtonModule } from 'primeng/button';
import { PanelModule } from 'primeng/panel';
import { MenuModule } from 'primeng/menu';
import { PasswordModule } from 'primeng/password';
import { DropdownModule } from 'primeng/dropdown';
import { RouterLink, RouterOutlet } from '@angular/router';
import { AlertService } from './services/alert.service';
import { AppService } from './services/app.service';
import { UserService } from './services/user.service';
import { ToastModule } from 'primeng/toast';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { ConfirmationService, MessageService } from 'primeng/api';
import { InputTextModule } from 'primeng/inputtext';
import { TextareaModule } from 'primeng/textarea';
import { CalendarModule } from 'primeng/calendar';
import { MultiSelectModule } from 'primeng/multiselect';
import { TreeModule } from 'primeng/tree';
import { DataViewModule } from 'primeng/dataview';
import { TagModule } from 'primeng/tag';
import { SelectButton } from 'primeng/selectbutton';
import { PaginatorModule } from 'primeng/paginator';
import { EditorModule } from 'primeng/editor';
import { TabsModule } from 'primeng/tabs';
import { SelectModule } from 'primeng/select';
import { ToggleSwitchModule } from 'primeng/toggleswitch';
import { MessageModule } from 'primeng/message';
import { AnimateOnScrollModule } from 'primeng/animateonscroll';
import { Ripple } from 'primeng/ripple';
import { FloatLabel } from 'primeng/floatlabel';
import { IconField } from 'primeng/iconfield';
import { InputIcon } from 'primeng/inputicon';
import { StyleClass } from 'primeng/styleclass';
import { InputMaskModule } from 'primeng/inputmask';
import { ScrollPanelModule } from 'primeng/scrollpanel';
import { FileUploadModule } from 'primeng/fileupload';
import { RadioButtonModule } from 'primeng/radiobutton';
import { GalleriaModule } from 'primeng/galleria';
import { InputNumberModule } from 'primeng/inputnumber';
import { InputGroupModule } from 'primeng/inputgroup';
import { ProgressSpinnerModule } from 'primeng/progressspinner';
import { TreeSelectModule } from 'primeng/treeselect';
import { ListboxModule } from 'primeng/listbox';
import { DrawerModule } from 'primeng/drawer';
import { TranslateModule } from '@ngx-translate/core';

@NgModule({
  declarations: [],
  imports: [
    RouterOutlet,
    RouterLink,
    CommonModule,
    ReactiveFormsModule,
    AnimateOnScrollModule,
    FormsModule,
    TableModule,
    InputTextModule,
    TextareaModule,
    DialogModule,
    CheckboxModule,
    ButtonModule,
    PanelModule,
    MenuModule,
    PasswordModule,
    DropdownModule,
    MenubarModule,
    ToastModule,
    ConfirmDialogModule,
    CalendarModule,
    MultiSelectModule,
    TreeModule,
    DataViewModule,
    TagModule,
    SelectButton,
    PaginatorModule,
    EditorModule,
    TabsModule,
    SelectModule,
    ToggleSwitchModule,
    MessageModule,
    Ripple,
    FloatLabel,
    StyleClass,
    IconField,
    InputIcon,
    InputMaskModule,
    ScrollPanelModule,
    FileUploadModule,
    RadioButtonModule,
    GalleriaModule,
    InputNumberModule,
    AvatarModule,
    InputGroupModule,
    ProgressSpinnerModule,
    TreeSelectModule,
    ListboxModule,
    DrawerModule,
    TranslateModule,
  ],
  exports: [
    RouterOutlet,
    RouterLink,
    CommonModule,
    ReactiveFormsModule,
    AnimateOnScrollModule,
    FormsModule,
    InputTextModule,
    TextareaModule,
    TableModule,
    DialogModule,
    CheckboxModule,
    ButtonModule,
    PanelModule,
    MenuModule,
    PasswordModule,
    DropdownModule,
    MenubarModule,
    ToastModule,
    ConfirmDialogModule,
    CalendarModule,
    MultiSelectModule,
    TreeModule,
    DataViewModule,
    TagModule,
    SelectButton,
    PaginatorModule,
    EditorModule,
    TabsModule,
    SelectModule,
    ToggleSwitchModule,
    MessageModule,
    Ripple,
    FloatLabel,
    StyleClass,
    IconField,
    InputIcon,
    InputMaskModule,
    ScrollPanelModule,
    FileUploadModule,
    RadioButtonModule,
    GalleriaModule,
    InputNumberModule,
    AvatarModule,
    InputGroupModule,
    ProgressSpinnerModule,
    TreeSelectModule,
    ListboxModule,
    DrawerModule,
    TranslateModule
  ],
  providers: [
    ConfirmationService,
    AppService,
    MessageService,
    AlertService,
    UserService,
  ],
})
export class SharedModule {}
