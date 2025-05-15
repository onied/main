import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:onied_mobile/repositories/user_repository.dart';

import 'profile_info_bloc_event.dart';
import 'profile_info_bloc_state.dart';

class ProfileInfoBloc extends Bloc<ProfileInfoBlocEvent, ProfileInfoBlocState> {
  final UserRepository repository;

  ProfileInfoBloc({required this.repository}) : super(LoadingState()) {
    on<LoadUser>(_onLoadUser);
    on<UpdateUserModel>(_onUpdateUserModel);
    // on<SaveUserInfo>(); // TODO: Save user to backend
    // on<SaveUserAvatar>(); // TODO: Save user avatar to backend
  }

  Future<void> _onLoadUser(
    LoadUser event,
    Emitter<ProfileInfoBlocState> emit,
  ) async {
    final profile = await repository.getProfile();
    if (profile == null) {
      emit(ErrorState(errorMessage: "User not authorized."));
    } else {
      emit(LoadedState(user: profile));
    }
  }

  Future<void> _onUpdateUserModel(
    UpdateUserModel event,
    Emitter<ProfileInfoBlocState> emit,
  ) async {
    emit(LoadedState(user: event.user));
  }
}
