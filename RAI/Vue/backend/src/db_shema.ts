import mongoose, {Document, Schema} from "mongoose";

const SALT_WORK_FACTOR = 10;
var bcrypt = require('bcrypt');


export interface IUser extends Document {
    username: string;
    password: string;
    comparePassword(candidatePassword: string): Promise<boolean>;
}

const userSchema = new Schema<IUser>({
    username: {type: String, unique: true, lowercase: true, required: true},
    password: {type: String, required: true}
});

userSchema.pre('save', async function(next: any) {
    var user = this as IUser;
    if (!user.isModified('password')) return next();

    try{
        const salt = await bcrypt.genSalt(SALT_WORK_FACTOR);
        user.password = await bcrypt.hash(user.password, salt);
        return next();
    } catch (err) {
        return next(err);
    }
});

userSchema.methods.comparePassword = async function(password: string) {
    return bcrypt.compare(password, this.password);
};

export interface ISavedStop extends Document {
    username: string;
    stopId: number;
}

const savedStopsSchema = new Schema<ISavedStop>({
    username: {type: String, required: true},
    stopId: {type: Number, required: true}
});
savedStopsSchema.index({ username: 1, stopId: 1 }, { unique: true });


export const UserSchema = userSchema;
export const User = mongoose.model<IUser>('user', userSchema);
export const SavedStops =  mongoose.model('saved', savedStopsSchema);
